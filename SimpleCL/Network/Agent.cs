using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SilkroadSecurityApi;
using SimpleCL.Model.Server;

namespace SimpleCL.Network
{
    public class Agent
    {
        private readonly Security _security = new Security();
        private readonly TransferBuffer _recvBuffer = new TransferBuffer(0x1000, 0, 0);
        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private string Ip;
        private ushort Port;

        public Agent(string ip, ushort port)
        {
            Ip = ip;
            Port = port;
        }

        public void Start()
        {
            try
            {
                _socket.Connect(Ip, Port);
                Console.WriteLine("Connected to agent " + Ip + ":" + Port);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to agent");
                Console.WriteLine(e);
            }

            Thread agLoop = new Thread(Loop);
            agLoop.Start();
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
                    Console.WriteLine(Utility.HexDump(_recvBuffer.Buffer, _recvBuffer.Offset, _recvBuffer.Size));

                    switch (packet.Opcode)
                    {
                        case Opcodes.HANDSHAKE:
                        case Opcodes.HANDSHAKE_ACCEPT:
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