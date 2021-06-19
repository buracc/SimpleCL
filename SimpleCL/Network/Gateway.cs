using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using SilkroadSecurityApi;
using SimpleCL.Model.Server;
using SimpleCL.Network.Enums;
using SimpleCL.Util;
using Timer = System.Timers.Timer;

namespace SimpleCL.Network
{
    public class Gateway
    {
        private readonly Security _security = new Security();
        private readonly TransferBuffer _recvBuffer = new TransferBuffer(0x1000, 0, 0);
        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public const byte TrsroVersion = 13;

        private List<string> Ips { get; }
        private ushort Port { get; }
        private int Version { get; }
        private byte Locale { get; }

        private Timer _timer = new Timer(5000);

        public Gateway(List<string> ips, ushort port, byte version, byte locale)
        {
            Ips = ips;
            Port = port;
            Version = version;
            Locale = locale;
        }

        public void Start()
        {
            string ip = Ips[new Random().Next(Ips.Count)];
            try
            {
                _socket.Connect(ip, Port);
                Console.WriteLine("Connected to gateway " + ip + ":" + Port);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect gateway");
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
            _timer.Elapsed += HeartBeat;

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
                    Console.WriteLine(Utility.HexDump(packet.GetBytes()));

                    switch (packet.Opcode)
                    {
                        case Opcodes.HANDSHAKE:
                        case Opcodes.HANDSHAKE_ACCEPT:
                            break;

                        case Opcodes.IDENTITY:
                            if (!_timer.Enabled)
                            {
                                _timer.Start();
                            }

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
                            Dictionary<string, SilkroadServer> servers = new Dictionary<string, SilkroadServer>();

                            while (packet.ReadUInt8() == 1)
                            {
                                byte farmId = packet.ReadUInt8();
                                string farmName = packet.ReadAscii();
                            }

                            Log("Servers:");
                            while (packet.ReadUInt8() == 1)
                            {
                                SilkroadServer server = new SilkroadServer(
                                    packet.ReadUInt16(),
                                    packet.ReadAscii(),
                                    (ServerCapacity) packet.ReadUInt8(),
                                    packet.ReadUInt8() == 1
                                );

                                servers[server.Name] = server;

                                Log(server.ToString());
                            }

                            Packet login = new Packet(Opcodes.Gateway.Request.LOGIN2, true);
                            login.WriteUInt8(Locale);
                            login.WriteAscii(Credentials.Username);
                            login.WriteAscii(Credentials.Password);
                            login.WriteUInt8Array(NetworkUtils.GetMacAddressBytes());
                            login.WriteUInt16(servers[Credentials.Server].Id);
                            login.WriteUInt8(1);

                            _security.Send(login);
                            break;

                        case Opcodes.Gateway.Response.LOGIN2:
                            byte[] key = {0x0F, 0x07, 0x3D, 0x20, 0x56, 0x62, 0xC9, 0xEB};
                            Blowfish blowfish = new Blowfish();
                            blowfish.Initialize(key);
                            byte[] encodedPasscode = Encoding.ASCII.GetBytes(Credentials.Passcode);
                            byte[] encryptedPasscode = blowfish.Encode(encodedPasscode);

                            Packet passcode = new Packet(Opcodes.Gateway.Request.PASSCODE, true);
                            passcode.WriteUInt8(4);
                            passcode.WriteUInt16(Credentials.Passcode.Length);
                            passcode.WriteUInt8Array(encryptedPasscode);

                            _security.Send(passcode);
                            break;

                        case Opcodes.Gateway.Response.PASSCODE:
                            packet.ReadUInt8();
                            byte passcodeResult = packet.ReadUInt8();
                            if (passcodeResult == 2)
                            {
                                byte attempts = packet.ReadUInt8();
                                Log("Invalid passcode. Attempts: [" + attempts + "/" + 3 + "]");
                                return;
                            }
                            break;

                        case Opcodes.Gateway.Response.AGENT_AUTH:
                            byte result = packet.ReadUInt8();
                            switch (result)
                            {
                                case 1:
                                    uint sessionId = packet.ReadUInt32();
                                    string agentIp = packet.ReadAscii();
                                    ushort agentPort = packet.ReadUInt16();

                                    Agent agent = new Agent(agentIp, agentPort, Locale, sessionId);
                                    agent.Start();
                                    break;

                                case 2:
                                    LoginErrorCode errorCode = (LoginErrorCode) packet.ReadUInt8();

                                    switch (errorCode)
                                    {
                                        case LoginErrorCode.InvalidCredentials:
                                            uint maxAttempts = packet.ReadUInt32();
                                            uint currentAttempts = packet.ReadUInt32();
                                            Log("Invalid credentials. Attempts: [" + currentAttempts +
                                                "/" + maxAttempts + "]");
                                            break;

                                        case LoginErrorCode.Blocked:
                                            LoginBlockType blockType = (LoginBlockType) packet.ReadUInt8();

                                            switch (blockType)
                                            {
                                                case LoginBlockType.Punishment:
                                                    string reason = packet.ReadAscii();
                                                    DateTime endDate = new DateTime(
                                                        packet.ReadUInt16(),
                                                        packet.ReadUInt16(),
                                                        packet.ReadUInt16(),
                                                        packet.ReadUInt16(),
                                                        packet.ReadUInt16(),
                                                        packet.ReadUInt16(),
                                                        packet.ReadUInt16()
                                                    );

                                                    Log("Account banned: " + reason + "\nEnd date: " +
                                                        endDate);
                                                    break;
                                                
                                                case LoginBlockType.AccountInspection:
                                                    Log("Unable to connect due to inspection.");
                                                    break;
                                            }

                                            break;
                                        
                                        case LoginErrorCode.AlreadyConnected:
                                            Log("Account is already connected.");
                                            return;
                                        
                                        case LoginErrorCode.IPLimit:
                                            Log("IP limit exceeded.");
                                            return;
                                        
                                        case LoginErrorCode.ServerIsFull:
                                            Log("Server is full.");
                                            return;
                                    }
                                    
                                    return;
                                
                                case 3:
                                    Log("Unhandled login result.");
                                    return;
                            }

                            return;
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

        public void HeartBeat(Object source, ElapsedEventArgs e)
        {
            _security.Send(new Packet(Opcodes.HEARTBEAT));
        }

        public void Log(string message)
        {
            Console.WriteLine("[Gateway] " + message);
        }
    }
}