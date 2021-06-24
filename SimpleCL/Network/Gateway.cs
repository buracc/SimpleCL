using System;
using System.Net.Sockets;
using System.Threading;

namespace SimpleCL.Network
{
    public class Gateway : Server
    {
        private readonly string _ip;
        private readonly ushort _port;

        public Gateway(string ip, ushort port)
        {
            _ip = ip;
            _port = port;
        }

        public void Start()
        {
            try
            {
                Log("Connecting to gateway " + _ip + ":" + _port);
                Socket.Connect(_ip, _port);
                Log("Connected to gateway");
            }
            catch (SocketException e)
            {
                Log("Unable to connect gateway " + _ip + ":" + _port);
                Console.WriteLine(e);
                return;
            }

            Thread gwLoop = new Thread(Loop);
            gwLoop.Start();
            Socket.Blocking = false;
            Socket.NoDelay = true;
        }
    }
}