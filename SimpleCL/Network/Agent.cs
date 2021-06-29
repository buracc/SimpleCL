using System;
using System.Net.Sockets;

namespace SimpleCL.Network
{
    public class Agent : Server
    {
        private readonly string _ip;
        private readonly ushort _port;
        public uint SessionId { get; }

        public Agent(string ip, ushort port, uint sessionId)
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
                Socket.Connect(_ip, _port);
                Log("Connected to agent");
            }
            catch (SocketException e)
            {
                Log("Unable to connect to agent " + _ip + ":" + _port);
                Console.WriteLine(e);
            }
            
            ServerThread.Start();
            Socket.Blocking = false;
            Socket.NoDelay = true;
        }
    }
}