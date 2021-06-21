using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly string _ip;
        private readonly ushort _port;

        private readonly Timer _timer = new Timer(5000);

        public Gateway(string ip, ushort port)
        {
            _ip = ip;
            _port = port;
        }

        public void Start()
        {
            try
            {
                Log("Connecting to gateway " + _ip + ":" + _port);
                Socket.Connect(_ip, _port);
                Log("Connected to gateway");
            }
            catch (SocketException e)
            {
                Log("Unable to connect gateway " + _ip + ":" + _port);
                Console.WriteLine(e);
                return;
            }

            Thread gwLoop = new Thread(Loop);
            gwLoop.Start();
            Socket.Blocking = false;
            Socket.NoDelay = true;
        }

        public void Loop()
        {
            _timer.Elapsed += HeartBeat;

            while (true)
            {
                SocketError success;

                if (!Socket.Connected)
                {
                    return;
                }

                try
                {
                    RecvBuffer.Size = Socket.Receive(RecvBuffer.Buffer, 0, RecvBuffer.Buffer.Length,
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

                if (RecvBuffer.Size < 0)
                {
                    return;
                }

                try
                {
                    Security.Recv(RecvBuffer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                List<Packet> incomingPackets = Security.TransferIncoming();
                if (incomingPackets == null)
                {
                    continue;
                }

                foreach (var packet in incomingPackets)
                {
                    if (Debug)
                    {
                        Log(packet.Opcode.ToString("X"), false);
                        Log("\n" + Utility.HexDump(packet.GetBytes()), false);
                    }
                    
                    if (packet.Opcode == Opcodes.IDENTITY && !_timer.Enabled)
                    {
                        _timer.Start();
                    }
                    
                    Notify(packet);
                }

                List<KeyValuePair<TransferBuffer, Packet>> outgoing = Security.TransferOutgoing();
                if (outgoing != null)
                {
                    foreach (var pair in outgoing)
                    {
                        TransferBuffer buffer = pair.Key;
                        success = SocketError.Success;

                        while (buffer.Offset != buffer.Size)
                        {
                            int n = Socket.Send(buffer.Buffer, buffer.Offset, buffer.Size - buffer.Offset,
                                SocketFlags.None, out success);
                            if (success != SocketError.Success && success != SocketError.WouldBlock)
                            {
                                break;
                            }

                            buffer.Offset += n;
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
            }
        }
    }
}