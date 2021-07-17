using System;
using System.Net.Sockets;

namespace SimpleCL.Network
{
    public class Agent : Server
    {
        private readonly string _ip;
        private readonly ushort _port;
        public uint SessionId { get; }

        public Agent(string ip, ushort port, uint sessionId) : base(ip, port)
        {
            _ip = ip;
            _port = port;
            SessionId = sessionId;
        }

        public void Start()
        {
            try
            {
                Log("Connecting to agent " + _ip + ":" + _port);
                // Socket.Connect(_ip, _port);
                
            }
            catch (SocketException e)
            {
                Log("Unable to connect to agent " + _ip + ":" + _port);
                Console.WriteLine(e);
                return;
            }
            
            Log("Connected to agent");
            
            ServerThread.Start();
            Socket.Blocking = false;
            Socket.NoDelay = true;
        }
    }
}