using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using SilkroadSecurityApi;
using SimpleCL.Network.Enums;
using Timer = System.Timers.Timer;

namespace SimpleCL.Network
{
    public class Agent : Server
    {
        private readonly string _ip;
        private readonly ushort _port;
        public uint SessionId { get; }

        private readonly Timer _timer = new Timer(5000);

        public Agent(string ip, ushort port, uint sessionId)
        {
            _ip = ip;
            _port = port;
            SessionId = sessionId;
        }

        public void Start()
        {
            try
            {
                Log("Connecting to agent " + _ip + ":" + _port);
                Socket.Connect(_ip, _port);
                Log("Connected to agent");
            }
            catch (SocketException e)
            {
                Log("Unable to connect to agent " + _ip + ":" + _port);
                Console.WriteLine(e);
            }

            Thread agLoop = new Thread(Loop);
            agLoop.Start();
            Socket.Blocking = false;
            Socket.NoDelay = true;
        }

        private void Loop()
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
                    if (packet.Opcode == Opcodes.Agent.Response.ENTITY_MOVEMENT)
                    {
                        continue;
                    }
                    
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