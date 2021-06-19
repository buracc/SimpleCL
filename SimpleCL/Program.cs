using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using SimpleCL.Network;
using SimpleCL.Network.Enums;

namespace SimpleCL
{
    internal class Program
    {
        private static List<string> GgGateways = new List<string>
        {
            "94.199.103.68",
            "94.199.103.69",
            "94.199.103.70",
        };

        private const ushort GgPort = 15779;

        public static void Main(string[] args)
        {
            Gateway gw = new Gateway(GgGateways, GgPort, Gateway.TrsroVersion, (byte) Locale.SRO_TR_Official_GameGami);
            
            gw.Start();
        }
    }
}