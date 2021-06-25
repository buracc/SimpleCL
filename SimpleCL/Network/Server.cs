using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Timers;
using SilkroadSecurityApi;
using SimpleCL.Enums;
using SimpleCL.Enums.Common;
using SimpleCL.Service;

namespace SimpleCL.Network
{
    public abstract class Server : IDisposable
    {
        protected readonly Security Security = new Security();
        protected readonly TransferBuffer RecvBuffer = new TransferBuffer(0x1000, 0, 0);
        protected readonly Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private readonly List<Service.Service> _services = new List<Service.Service>();
        private readonly List<Tuple<ushort, PacketHandler>> _handlers = new List<Tuple<ushort, PacketHandler>>();

        private delegate void PacketHandler(Server server, Packet packet);

        private readonly Timer _timer = new Timer(5000);

        private bool _disposing;

        public bool Debug { get; set; }

        public void RegisterService(Service.Service service)
        {
            if (_services.Contains(service))
            {
                return;
            }

            _services.Add(service);

            foreach (var method in service.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                PacketHandlerAttribute packetHandlerMethod = method.GetCustomAttribute<PacketHandlerAttribute>();
                if (packetHandlerMethod != null)
                {
                    PacketHandler handler =
                        (PacketHandler) Delegate.CreateDelegate(typeof(PacketHandler), service, method);
                    _handlers.Add(Tuple.Create(packetHandlerMethod.Opcode, handler));
                }
            }
        }

        public void RemoveService<T>(T service) where T : Service.Service
        {
            _services.Remove(service);
            _handlers.RemoveAll(x => x.Item2.Target?.GetType() == typeof(T));
        }

        private void Notify(Packet packet)
        {
            foreach (var (opcode, handler) in _handlers)
            {
                if (packet.Opcode == opcode)
                {
                    handler.Invoke(this, packet);
                }
            }
        }

        public void Log(string message, bool toGui = true)
        {
            string logMsg = "[" + GetType().Name + "] " + message;
            if (toGui && Program.Gui != null)
            {
                Program.Gui.Log(logMsg);
            }
            else
            {
                Console.WriteLine(logMsg);
            }
        }

        public void Inject(Packet packet)
        {
            Security.Send(packet);
        }

        private void HeartBeat(Object source, ElapsedEventArgs e)
        {
            Inject(new Packet(Opcodes.HEARTBEAT));
            Program.Gui.RefreshGui();
        }

        public void Disconnect()
        {
            Program.Gui.ToggleControls(true);
            Dispose();
        }

        public void Dispose()
        {
            Socket?.Dispose();
            _disposing = true;
        }

        protected void Loop()
        {
            _timer.Elapsed += HeartBeat;

            while (true)
            {
                SocketError success;

                if (!Socket.Connected)
                {
                    break;
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
                    break;
                }

                if (success != SocketError.Success && success != SocketError.WouldBlock)
                {
                    break;
                }

                if (RecvBuffer.Size < 0)
                {
                    break;
                }

                try
                {
                    Security.Recv(RecvBuffer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    break;
                }

                List<Packet> incomingPackets = Security.TransferIncoming();
                if (incomingPackets == null)
                {
                    continue;
                }

                foreach (var packet in incomingPackets)
                {
                    if (Debug || this is Agent && Program.Gui.DebugAgent() || this is Gateway && Program.Gui.DebugGateway())
                    {
                        DebugPacket(packet);
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

            if (_disposing)
            {
                return;
            }

            Disconnect();
        }

        public void DebugPacket(Packet packet, bool toGui = false)
        {
            Log(packet.Opcode.ToString("X"), toGui);
            Log("\n" + Utility.HexDump(packet.GetBytes()), toGui);
        }
    }
}