using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Timers;
using SimpleCL.SilkroadSecurityApi;
using SimpleCL.Enums.Commons;
using SimpleCL.Interaction;
using SimpleCL.Services;
using Timer = System.Timers.Timer;

namespace SimpleCL.Network
{
    public abstract class Server : IDisposable
    {
        public readonly Thread ServerThread;
        protected readonly Security Security = new Security();
        protected readonly TransferBuffer RecvBuffer = new TransferBuffer(8192, 0, 0);
        protected readonly Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private readonly List<Service> _services = new List<Service>();
        private readonly List<Tuple<ushort, PacketHandler>> _handlers = new List<Tuple<ushort, PacketHandler>>();
        private readonly ConcurrentQueue<Action> _actions = new ConcurrentQueue<Action>();

        private delegate void PacketHandler(Server server, Packet packet);

        private readonly Timer _timer = new Timer(6666);
        private bool _disposed;

        protected Server()
        {
            ServerThread = new Thread(Loop) {IsBackground = true};
        }

        public void RegisterService(Service service)
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

        public void RemoveService<T>(T service) where T : Service
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
            if (toGui && SimpleCL.Gui != null)
            {
                SimpleCL.Gui.Log(logMsg);
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

            SimpleCL.Gui.RefreshGui();
        }

        public void Disconnect()
        {
            SimpleCL.Gui.ToggleControls(true);
            Dispose();
        }

        public void Dispose()
        {
            _actions.Enqueue(() =>
            {
                _services.Clear();
                _handlers.Clear();
                Socket?.Dispose();
                _disposed = true;
            });
        }

        private void Loop()
        {
            _timer.Elapsed += HeartBeat;

            while (true)
            {
                if (_actions.TryDequeue(out var action))
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    continue;
                }

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

                if (InteractionQueue.PacketQueue.TryDequeue(out var queuedPacket))
                {
                    try
                    {
                        Inject(queuedPacket);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                List<Packet> incomingPackets = Security.TransferIncoming();
                if (incomingPackets == null)
                {
                    continue;
                }

                foreach (var packet in incomingPackets)
                {
                    if (packet.Opcode == Opcodes.HANDSHAKE || packet.Opcode == Opcodes.HANDSHAKE_ACCEPT)
                    {
                        continue;
                    }

                    if (this is Agent && SimpleCL.Gui.DebugAgent() ||
                        this is Gateway && SimpleCL.Gui.DebugGateway())
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

            if (_disposed)
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