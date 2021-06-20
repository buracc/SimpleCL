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
        protected readonly Security _security = new Security();
        protected readonly TransferBuffer _recvBuffer = new TransferBuffer(0x1000, 0, 0);
        protected readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private List<Service.Service> Services = new List<Service.Service>();
        private readonly List<Tuple<ushort, PacketHandler>> Handlers = new List<Tuple<ushort, PacketHandler>>();

        private delegate void PacketHandler(Server server, Packet packet);

        private bool _debug;

        public bool Debug
        {
            get => _debug;
            set => _debug = value;
        }

        public void RegisterService(Service.Service service)
        {
            if (Services.Contains(service))
            {
                return;
            }
            
            Services.Add(service);

            foreach (var method in service.GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                PacketHandlerAttribute packetHandlerMethod = method.GetCustomAttribute<PacketHandlerAttribute>();
                if (packetHandlerMethod != null)
                {
                    PacketHandler handler = (PacketHandler) Delegate.CreateDelegate(typeof(PacketHandler), service, method);
                    Handlers.Add(Tuple.Create(packetHandlerMethod.Opcode, handler));
                }
            }
        }

        public void RemoveService<T>(T service) where T: Service.Service
        {
            Services.Remove(service);
            Handlers.RemoveAll(x => x.Item2.Target?.GetType() == typeof(T));
        }

        public void Notify(Packet packet)
        {
            foreach (var (opcode, handler) in Handlers)
            {
                if (packet.Opcode == opcode)
                {
                    handler.Invoke(this, packet);
                }
            }
        }
        
        public void Log(string message)
        {
            Console.WriteLine("[" + GetType().Name + "] " + message);
        }

        public void Inject(Packet packet)
        {
            _security.Send(packet);
        }
        
        public void HeartBeat(Object source, ElapsedEventArgs e)
        { 
            Inject(new Packet(Opcodes.HEARTBEAT));
        }

        public void Close()
        {
            _socket.Close();
        }
    }
}