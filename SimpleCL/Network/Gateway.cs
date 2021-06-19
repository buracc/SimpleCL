using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SilkroadSecurityApi;
using SimpleCL.Model.Server;

namespace SimpleCL.Network
{
    public class Gateway
    {
        private readonly Security _security = new Security();
        private readonly TransferBuffer _recvBuffer = new TransferBuffer(0x1000, 0, 0);
        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public const byte TrsroVersion = 13;

        private string Ip { get; }
        private int Port { get; }
        private UInt32 Version { get; }
        private byte Locale { get; }

        public Gateway(string ip, int port, byte version, byte locale)
        {
            Ip = ip;
            Port = port;
            Version = version;
            Locale = locale;
        }

        public void Start()
        {
            try
            {
                _socket.Connect(Ip, Port);
                Console.WriteLine("Connected to gateway");
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect");
                Console.WriteLine(e);
                return;
            }
            
            Thread gwLoop = new Thread(Loop);
            gwLoop.Start();
            _socket.Blocking = false;
            _socket.NoDelay = true;
        }

        public void Loop()
        {
            while (true)
            {
                SocketError success;

                try
                {
                    _recvBuffer.Size = _socket.Receive(_recvBuffer.Buffer, 0, _recvBuffer.Buffer.Length,
                        SocketFlags.None,
                        out success);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                if (success != SocketError.Success && success != SocketError.WouldBlock)
                {
                    return;
                }

                if (_recvBuffer.Size < 0)
                {
                    return;
                }

                try
                {
                    _security.Recv(_recvBuffer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                List<Packet> incomingPackets = _security.TransferIncoming();
                if (incomingPackets == null)
                {
                    continue;
                }

                foreach (var packet in incomingPackets)
                {
                    Console.WriteLine(packet.Opcode.ToString("X"));

                    switch (packet.Opcode)
                    {
                        case Opcodes.HANDSHAKE:
                        case Opcodes.HANDSHAKE_ACCEPT:
                            break;
                        
                        case Opcodes.IDENTITY:
                            Packet identity = new Packet(Opcodes.Gateway.Request.PATCH, true);
                            identity.WriteUInt8(Locale);
                            identity.WriteAscii("SR_Client");
                            identity.WriteUInt32(Version);
                            _security.Send(identity);
                            break;
                        
                        case Opcodes.Gateway.Response.PATCH:
                            _security.Send(new Packet(Opcodes.Gateway.Request.SERVERLIST, true));
                            break;
                        
                        case Opcodes.Gateway.Response.SERVERLIST:
                            while (packet.ReadUInt8() == 1)
                            {
                                byte farmId = packet.ReadUInt8();
                                string farmName = packet.ReadAscii();
                            }

                            while (packet.ReadUInt8() == 1)
                            {
                                SilkroadServer server = new SilkroadServer(packet.ReadUInt16(), packet.ReadAscii(),
                                    (ServerCapacity) packet.ReadUInt8(), packet.ReadUInt8() == 1);

                                Console.WriteLine(server.ToString());
                            }

                            Packet login = new Packet(Opcodes.Gateway.Request.LOGIN2, true);
                            login.WriteUInt8(Locale);
                            login.WriteAscii(Program.Username);
                            login.WriteAscii(Program.Password);
                            login.WriteUInt16(123);
                            login.WriteUInt16(123);
                            login.WriteUInt16(123);
                            login.WriteUInt16(356);
                            login.WriteUInt8(1);
                            
                            _security.Send(login);
                            break;
                        
                        case Opcodes.Gateway.Response.LOGIN2:
                            byte[] key = { 0x0F, 0x07, 0x3D, 0x20, 0x56, 0x62, 0xC9, 0xEB };
                            Blowfish blowfish = new Blowfish();
                            blowfish.Initialize(key);
                            byte[] encodedPasscode = Encoding.ASCII.GetBytes(Program.Passcode);
                            byte[] encryptedPasscode = blowfish.Encode(encodedPasscode);

                            Packet passcode = new Packet(Opcodes.Gateway.Request.PASSCODE, true);
                            passcode.WriteUInt8(4);
                            passcode.WriteUInt16(Program.Passcode.Length);
                            passcode.WriteUInt8Array(encryptedPasscode);

                            _security.Send(passcode);
                            break;
                        
                        case Opcodes.Gateway.Response.PASSCODE:
                            Console.WriteLine("Passcode entered");
                            break;
                        
                        case Opcodes.Gateway.Response.AGENT_AUTH:
                            Console.WriteLine("Agent auth");
                            break;
                    }
                }

                List<KeyValuePair<TransferBuffer, Packet>> outgoing = _security.TransferOutgoing();
                if (outgoing != null)
                {
                    foreach (var pair in outgoing)
                    {
                        TransferBuffer buffer = pair.Key;
                        success = SocketError.Success;

                        while (buffer.Offset != buffer.Size)
                        {
                            int n = _socket.Send(buffer.Buffer, buffer.Offset, buffer.Size - buffer.Offset,
                                SocketFlags.None, out success);
                            if (success != SocketError.Success && success != SocketError.WouldBlock)
                            {
                                break;
                            }

                            buffer.Offset += n;
                            Thread.Sleep(1);
                        }

                        if (success != SocketError.Success)
                        {
                            break;
                        }
                    }

                    if (success != SocketError.Success)
                    {
                        break;
                    }
                }

                Thread.Sleep(1);
            }
        }
    }
}