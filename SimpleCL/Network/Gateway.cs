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
        public string SocksIp { get; }
        public int SocksPort { get; }
        public string SocksUser { get; }
        public string SocksPass { get; }
        public bool UseClient { get; }

        protected SilkroadClient SilkroadClient { get; }

        public Gateway(string ip,
            ushort port,
            string socksIp = null,
            int socksPort = 0,
            string socksUser = null,
            string socksPass = null,
            bool useClient = false,
            Locale locale = Locale.SRO_TR_Official_GameGami
        ) : base(ip, port, socksIp, socksPort, socksUser, socksPass, useClient)
        {
            SocksIp = socksIp;
            SocksPort = socksPort;
            SocksUser = socksUser;
            SocksPass = socksPass;
            UseClient = useClient;

            if (!useClient)
            {
                return;
            }

            Proxy = new Proxy(this, "127.0.0.1", 60069);
            Proxy.Start();
            
            SilkroadClient = new SilkroadClient(@"C:\Program Files (x86)\SilkroadTR", locale);
            new Injector(SilkroadClient.Launch(), "simplecl.dll").Inject();
        }
    }
}