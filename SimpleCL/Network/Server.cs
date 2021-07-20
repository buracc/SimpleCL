using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using SimpleCL.Enums.Commons;
using SimpleCL.Interaction;
using SimpleCL.SecurityApi;
using SimpleCL.Services;
using SimpleCL.Util.Extension;
using Timer = System.Timers.Timer;

namespace SimpleCL.Network
{
    public abstract class Server : IDisposable
    {
        public readonly Thread ServerThread;
        protected readonly Security RemoteSecurity = new();
        protected readonly Security LocalSecurity = new();

        protected readonly TransferBuffer RecvBuffer = new(4096, 0, 0);
        protected readonly TransferBuffer LocalRecvBuffer = new(4096, 0, 0);

        protected readonly Socket Socket;

        private readonly List<Service> _services = new();
        private readonly List<Tuple<ushort, PacketHandler>> _handlers = new();
        private readonly ConcurrentQueue<Action> _actions = new();
        
        private delegate void PacketHandler(Server server, Packet packet);

        private readonly Timer _timer = new(6666);
        private bool _disposed;

        protected Server(
            string ip,
            ushort port,
            string proxyIp,
            int proxyPort,
            string proxyUser,
            string proxyPass,
            bool launchClient
        )
        {
            ServerThread = new Thread(Loop);
            
            var sb = new StringBuilder($"Connecting to [{ip}:{port}]");
            if (proxyIp != null)
            {
                sb.Append($" || Proxy: [{proxyIp}:{proxyPort}]");

                if (proxyUser != null)
                {
                    sb.Append($"[{proxyUser}:{proxyPass}]");
                }
            }
            
            Log(sb.ToString());
            
            try
            {
                if (proxyIp != null && proxyPort != 0)
                {
                    Socket = Socks.Connect(proxyIp, proxyPort, ip, port, proxyUser, proxyPass);
                }
                else
                {
                    Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Socket.Connect(ip, port);
                }
                
                Log("Connection successful");
                
                Socket.Blocking = false;
                Socket.NoDelay = true;
            }
            catch (Exception e)
            {
                Log("Connection failed");
                Console.WriteLine(e);
            }
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
                var packetHandlerMethod = method.GetCustomAttribute<PacketHandlerAttribute>();
                if (packetHandlerMethod == null)
                {
                    continue;
                }

                var handler =
                    (PacketHandler) Delegate.CreateDelegate(typeof(PacketHandler), service, method);
                _handlers.Add(Tuple.Create(packetHandlerMethod.Opcode, handler));
            }
        }

        public void RemoveService<T>(T service) where T : Service
        {
            _services.Remove(service);
            _handlers.RemoveAll(x => x.Item2.Target?.GetType() == typeof(T));
        }

        protected void Notify(Packet packet)
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
            if (toGui && Program.Gui != null)
            {
                Program.Gui.Log(message, GetType().Name);
            }
            else
            {
                Console.WriteLine(message);
            }
        }

        public void Inject(Packet packet)
        {
            RemoteSecurity.Send(packet);
        }

        private void HeartBeat(object source, ElapsedEventArgs e)
        {
            Inject(new Packet(Opcode.HEARTBEAT));
        }

        public void Disconnect()
        {
            Program.Gui.ToggleControls(true);
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
                    RemoteSecurity.Recv(RecvBuffer);
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
                        continue;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                var incomingPackets = RemoteSecurity.TransferIncoming();
                if (incomingPackets != null && incomingPackets.IsNotEmpty())
                {
                    foreach (var packet in incomingPackets)
                    {
                        if (packet.Opcode is Opcode.HANDSHAKE or Opcode.HANDSHAKE_ACCEPT)
                        {
                            continue;
                        }
                        
                        if (this is Agent && Program.Gui.DebugAgent() ||
                            this is Gateway && Program.Gui.DebugGateway())
                        {
                            LogPacket(packet, $"Remote -> {GetType().Name}");
                        }

                        if (packet.Opcode == Opcode.IDENTITY && !_timer.Enabled)
                        {
                            _timer.Start();
                        }

                        Notify(packet);
                        LocalSecurity.Send(packet);
                    }
                }

                var outgoing = RemoteSecurity.TransferOutgoing();
                if (outgoing != null)
                {
                    foreach (var kvp in outgoing)
                    {
                        if (this is Agent && Program.Gui.DebugAgent() ||
                            this is Gateway && Program.Gui.DebugGateway())
                        {
                            LogPacket(kvp.Value, $"{GetType().Name} -> Remote");
                        }

                        var buffer = kvp.Key;
                        success = SocketError.Success;

                        while (buffer.Offset != buffer.Size)
                        {
                            var n = Socket.Send(buffer.Buffer, buffer.Offset, buffer.Size - buffer.Offset,
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

        protected void LogPacket(Packet packet, string context = "Server")
        {
            if (Program.Gui.FilteredOpcodes.Contains(packet.Opcode.ToString("X")) ||
                Program.Gui.FilteredOpcodes.Contains("0"))
            {
                Program.Gui.LogPacket($"[{context}][{packet.Opcode:X}]\n{Utility.HexDump(packet.GetBytes())}");
                Console.WriteLine($"[{context}][{packet.Opcode:X}]\n{Utility.HexDump(packet.GetBytes())}");
            }
        }
        
        public void Start()
        {
            if (Socket is not {Connected: true})
            {
                Disconnect();
                return;
            }
            
            ServerThread.Start();
        }
    }
}