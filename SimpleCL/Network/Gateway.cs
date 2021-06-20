using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using SilkroadSecurityApi;
using SimpleCL.Network.Enums;
using Timer = System.Timers.Timer;

namespace SimpleCL.Network
{
    public class Gateway: Server
    {
        public const byte TrsroVersion = 13;
        private List<string> Ips { get; }
        private ushort Port { get; }

        private Timer _timer = new Timer(5000);

        public Gateway(List<string> ips, ushort port)
        {
            Ips = ips;
            Port = port;
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

                if (!_socket.Connected)
                {
                    return;
                }

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
                    if (Debug)
                    {
                        Log(packet.Opcode.ToString("X"));
                        Log(Utility.HexDump(packet.GetBytes()));
                    }
                    
                    if (packet.Opcode == Opcodes.IDENTITY && !_timer.Enabled)
                    {
                        _timer.Start();
                    }
                    
                    Notify(packet);
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