using System;
using System.Net.Sockets;

namespace SimpleCL.Network
{
    public class Gateway : Server
    {
        public string ProxyIp { get; }
        public int ProxyPort { get; }
        public string ProxyUser { get; }
        public string ProxyPass { get; }

        public Gateway(string ip,
            ushort port,
            string proxyIp = null,
            int proxyPort = 0,
            string proxyUser = null,
            string proxyPass = null
        ) : base(ip, port, proxyIp, proxyPort, proxyUser, proxyPass)
        {
            ProxyIp = proxyIp;
            ProxyPort = proxyPort;
            ProxyUser = proxyUser;
            ProxyPass = proxyPass;
        }
    }
}