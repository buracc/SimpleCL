using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SimpleCL.Enums.Commons;
using SimpleCL.SecurityApi;

namespace SimpleCL.Network
{
    public class Proxy
    {
        private readonly TcpListener _localServer;
        private TcpClient LocalClient { get; set; }
        private readonly Security _security = new();
        private readonly TransferBuffer _recvBuffer = new(8192, 0, 0);

        private readonly Server _context;

        public Proxy(Server context, string ip, int port)
        {
            _context = context;
            _localServer = new TcpListener(IPAddress.Parse(ip), port);
        }

        public bool IsConnected()
        {
            return LocalClient?.Connected ?? false;
        }

        public void Start()
        {
            var initThread = new Thread(InitThread);
            initThread.Start();
        }

        public void Stop()
        {
            LocalClient?.Close();
        }

        public void Receive(Packet packet)
        {
            _security.Send(packet);
        }

        private void InitThread()
        {
            _security.GenerateSecurity(true, true, true);
            _localServer.Start();

            LocalClient = _localServer.AcceptTcpClient();
            _localServer.Stop();

            _localServer.Server.NoDelay = true;
            _localServer.Server.Blocking = false;

            _context.Log("Client successfully connected");

            _context.Start();

            var loopThread = new Thread(ProxyLoop);
            loopThread.Start();
            loopThread.Join();
        }

        private void ProxyLoop()
        {
            var clientStream = LocalClient.GetStream();

            while (LocalClient.Connected)
            {
                if (_context.Disposed)
                {
                    break;
                }

                if (clientStream.DataAvailable)
                {
                    _recvBuffer.Offset = 0;
                    _recvBuffer.Size = clientStream.Read(_recvBuffer.Buffer, 0, _recvBuffer.Buffer.Length);
                    _security.Recv(_recvBuffer);
                }

                var incomingPackets = _security.TransferIncoming();
                if (incomingPackets != null)
                {
                    foreach (var packet in incomingPackets)
                    {
                        // Bot and Security will handle these
                        if (packet.Opcode is
                            Opcode.HANDSHAKE or
                            Opcode.HANDSHAKE_ACCEPT or
                            Opcode.IDENTITY or
                            Opcode.HEARTBEAT or
                            Opcode.Gateway.Request.PATCH
                        )
                        {
                            continue;
                        }

                        if (_context is Agent && Program.Gui.DebugAgent() && Program.Gui.DebugClient() &&
                            Program.Gui.DebugOutgoing() ||
                            _context is Gateway && Program.Gui.DebugGateway() && Program.Gui.DebugClient() &&
                            Program.Gui.DebugOutgoing())
                        {
                            _context.LogPacket(packet, $"Client -> {_context.GetType().Name}");
                        }

                        // Forward to server
                        _context.Inject(packet);
                    }
                }

                var outgoingPackets = _security.TransferOutgoing();
                if (outgoingPackets != null)
                {
                    foreach (var keyValuePair in outgoingPackets)
                    {
                        var packet = keyValuePair.Value;
                        var buffer = keyValuePair.Key;

                        if (_context is Agent && Program.Gui.DebugAgent() && Program.Gui.DebugClient() &&
                            Program.Gui.DebugIncoming() ||
                            _context is Gateway && Program.Gui.DebugGateway() && Program.Gui.DebugClient() &&
                            Program.Gui.DebugIncoming())
                        {
                            _context.LogPacket(packet, $"{_context.GetType().Name} -> Client");
                        }

                        // Try to send to client
                        try
                        {
                            clientStream.Write(buffer.Buffer, 0, buffer.Size);
                        }
                        catch (Exception e)
                        {
                            // Disconnect if client is no longer active
                            _context.Log("Client crashed, enabling clientless");
                            Console.WriteLine(e);
                            Console.WriteLine(outgoingPackets.Count);
                            _context.DebugPacket(packet);
                            break;
                        }

                        _context.Notify(packet);
                        Thread.Sleep(1);
                    }
                }

                Thread.Sleep(1);
            }

            Stop();
        }
    }
}