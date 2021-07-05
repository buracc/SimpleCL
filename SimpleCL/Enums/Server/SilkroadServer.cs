using System.Collections.Generic;
using SimpleCL.Enums.Commons;

namespace SimpleCL.Enums.Server
{
    public class SilkroadServer
    {
        public static readonly SilkroadServer Trsro = new(
            "TRSRO",
            Locale.SRO_TR_Official_GameGami,
            new[]
            {
                "94.199.103.68",
                "94.199.103.69",
                "94.199.103.70"
            });
        
        public static readonly SilkroadServer Isro = new(
            "iSRO",
            Locale.SRO_Global_TestBed,
            new[]
            {
                "45.58.13.127",
                "45.58.13.21"
            });
        
        public string Name { get; }
        public Locale Locale { get; }
        public string[] GatewayIps { get; }

        private SilkroadServer(string name, Locale locale, string[] gatewayIps)
        {
            Name = name;
            Locale = locale;
            GatewayIps = gatewayIps;
        }

        public override string ToString()
        {
            return Name;
        }

        public static IEnumerable<SilkroadServer> Values
        {
            get { yield return Trsro; }
        }
    }
}