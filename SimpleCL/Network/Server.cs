using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection;
using System.Timers;
using SilkroadSecurityApi;
using SimpleCL.Network.Enums;
using SimpleCL.Service;

namespace SimpleCL.Network
{
    public abstract class Server
    {
        protected readonly Security Security = new Security();
        protected readonly TransferBuffer RecvBuffer = new TransferBuffer(0x1000, 0, 0);
        protected readonly Socket Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private readonly List<Service.Service> _services = new List<Service.Service>();
        private readonly List<Tuple<ushort, PacketHandler>> _handlers = new List<Tuple<ushort, PacketHandler>>();

        private delegate void PacketHandler(Server server, Packet packet);

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
                    PacketHandler handler = (PacketHandler) Delegate.CreateDelegate(typeof(PacketHandler), service, method);
                    _handlers.Add(Tuple.Create(packetHandlerMethod.Opcode, handler));
                }
            }
        }

        public void RemoveService<T>(T service) where T: Service.Service
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
        
        protected void HeartBeat(Object source, ElapsedEventArgs e)
        { 
            Inject(new Packet(Opcodes.HEARTBEAT));
        }

        public void Close()
        {
            Program.Gui.ToggleLoginButton(true);
            Socket.Close();
        }
    }
}