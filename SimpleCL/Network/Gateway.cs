using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SimpleCL.Client;
using SimpleCL.Enums.Commons;
using SimpleCL.SecurityApi;

namespace SimpleCL.Network
{
    public class Gateway : Server
    {
        public string ProxyIp { get; }
        public int ProxyPort { get; }
        public string ProxyUser { get; }
        public string ProxyPass { get; }
        public bool LaunchClient { get; }
        protected TcpClient LocalClient { get; set; }

        protected SilkroadClient SilkroadClient { get; }

        public Gateway(string ip,
            ushort port,
            string proxyIp = null,
            int proxyPort = 0,
            string proxyUser = null,
            string proxyPass = null,
            bool launchClient = false,
            Locale locale = Locale.SRO_TR_Official_GameGami
        ) : base(ip, port, proxyIp, proxyPort, proxyUser, proxyPass, launchClient)
        {
            ProxyIp = proxyIp;
            ProxyPort = proxyPort;
            ProxyUser = proxyUser;
            ProxyPass = proxyPass;
            LaunchClient = launchClient;

            if (!launchClient)
            {
                return;
            }

            new Thread(LocalThread).Start();
            SilkroadClient = new SilkroadClient(@"C:\Program Files (x86)\SilkroadTR", locale);
            new Injector(SilkroadClient.Launch(), @"C:\Users\burak\Desktop\fuck_burak\Release\fuck_burak.dll").Inject();
        }

        protected void LocalThread()
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 60069);
            LocalSecurity.GenerateSecurity(true, true, true);
            listener.Start();
            LocalClient = listener.AcceptTcpClient();
            listener.Stop();

            Start();
            Log("Client successfully connected");

            var gwThread = new Thread(GatewayThread);
            gwThread.Start();
            gwThread.Join();
        }

        protected void GatewayThread()
        {
            var clientStream = LocalClient.GetStream();

            while (true)
            {
                if (clientStream.DataAvailable)
                {
                    LocalRecvBuffer.Offset = 0;
                    LocalRecvBuffer.Size = clientStream.Read(LocalRecvBuffer.Buffer, 0, LocalRecvBuffer.Buffer.Length);
                    LocalSecurity.Recv(LocalRecvBuffer);
                }

                //
                // todo:
                // redirect packets sent from client to remote
                // right now nothing happens if you interact with the client, everything needs to happen through bot
                // 
                var incomingPackets = LocalSecurity.TransferIncoming();
                if (incomingPackets != null)
                {
                    foreach (var packet in incomingPackets)
                    {
                        if (packet.Opcode is 0x5000 or 0x9000 or 0x2001)
                        {
                            continue;
                        }

                        if (Program.Gui.DebugGateway())
                        {
                            LogPacket(packet, "Client -> Gateway");
                        }
                    }
                }

                var outgoingPackets = LocalSecurity.TransferOutgoing();
                if (outgoingPackets != null)
                {
                    foreach (var keyValuePair in outgoingPackets)
                    {
                        var packet = keyValuePair.Value;
                        var buffer = keyValuePair.Key;

                        if (Program.Gui.DebugGateway())
                        {
                            LogPacket(packet, "Gateway -> Client");
                        }

                        clientStream.Write(buffer.Buffer, 0, buffer.Size);
                        Thread.Sleep(15); // Seems like the client crashes if you write to it too fast
                    }
                }

                Thread.Sleep(1);
            }
        }
    }
}