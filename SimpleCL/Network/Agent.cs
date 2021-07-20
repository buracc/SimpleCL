using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleCL.Network
{
    public class Agent : Server
    {
        public uint SessionId { get; }
        protected TcpClient LocalClient { get; set; }

        public Agent(string ip, ushort port, uint sessionId, Gateway gateway)
            : base(ip, port, gateway.ProxyIp, gateway.ProxyPort, gateway.ProxyUser, gateway.ProxyPass,
                gateway.LaunchClient)
        {
            SessionId = sessionId;

            if (!gateway.LaunchClient)
            {
                Start();
                return;
            }

            new Thread(LocalThread).Start();
        }

        protected void LocalThread()
        {
            var listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 60070);
            LocalSecurity.GenerateSecurity(true, true, true);
            listener.Start();
            LocalClient = listener.AcceptTcpClient(); listener.Stop();

            Log("Client successfully connected");

            Start();

            var agentThread = new Thread(AgentThread);
            agentThread.Start();
            agentThread.Join();
        }

        protected void AgentThread()
        {
            var localStream = LocalClient.GetStream();
            
            while (true)
            {
                if (localStream.DataAvailable)
                {
                    LocalRecvBuffer.Offset = 0;
                    LocalRecvBuffer.Size = localStream.Read(LocalRecvBuffer.Buffer, 0, LocalRecvBuffer.Buffer.Length);
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

                        if (Program.Gui.DebugAgent())
                        {
                            LogPacket(packet, "Client -> Agent");
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

                        if (Program.Gui.DebugAgent())
                        {
                            LogPacket(packet, "Agent -> Client");
                        }

                        localStream.Write(buffer.Buffer, 0, buffer.Size);
                        Thread.Sleep(15); // Seems like the client crashes if you write to it too fast
                    }
                }

                Thread.Sleep(1);
            }
        }
    }
}