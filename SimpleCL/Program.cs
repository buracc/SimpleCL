using System.Collections.Generic;
using SimpleCL.Network;
using SimpleCL.Service.Login;

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
            Gateway gw = new Gateway(GgGateways, GgPort);
            gw.RegisterService(new LoginService());
            
            gw.Start();
        }
    }
}