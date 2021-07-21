using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleCL.Network
{
    public class Agent : Server
    {
        public uint SessionId { get; }

        public Agent(string ip, ushort port, uint sessionId, Gateway gateway)
            : base(ip, port, gateway.SocksIp, gateway.SocksPort, gateway.SocksUser, gateway.SocksPass,
                gateway.UseClient)
        {
            SessionId = sessionId;

            if (!gateway.UseClient)
            {
                Start();
                return;
            }
            
            Proxy = new Proxy(this, "127.0.0.1", 60070);
            Proxy.Start();
        }
    }
}