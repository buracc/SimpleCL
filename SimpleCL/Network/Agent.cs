namespace SimpleCL.Network
{
    public class Agent : Server
    {
        public uint SessionId { get; }

        public Agent(string ip, ushort port, uint sessionId, Gateway gateway)
            : base(ip, port, gateway.ProxyIp, gateway.ProxyPort, gateway.ProxyUser, gateway.ProxyPass)
        {
            SessionId = sessionId;
        }
    }
}