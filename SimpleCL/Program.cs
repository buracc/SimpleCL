using SimpleCL.Network;

namespace SimpleCL
{
    internal class Program
    {
        public const string Username = "";
        public const string Password = "";
        public const string Passcode = "";

        private const string GgGateway = "srogw01.gamegami.com";
        private const int GgPort = 15779;
        
        public static void Main(string[] args)
        {
            Gateway gw = new Gateway(GgGateway, GgPort, Gateway.TrsroVersion, (byte) Locale.SRO_TR_Official_GameGami);
            
            gw.Start();
        }
    }
}