﻿using System;
using System.Net.Sockets;

namespace SimpleCL.Network
{
    public class Gateway : Server
    {
        private readonly string _ip;
        private readonly ushort _port;

        public Gateway(string ip, ushort port) : base(ip, port)
        {
            _ip = ip;
            _port = port;
        }

        public void Start()
        {
            try
            {
                Log("Connecting to gateway " + _ip + ":" + _port);
                // Socket.Connect(_ip, _port);
                
            }
            catch (SocketException e)
            {
                Log("Unable to connect gateway " + _ip + ":" + _port);
                Console.WriteLine(e);
                return;
            }

            Log("Connected to gateway");
            ServerThread.Start();
            Socket.Blocking = false;
            Socket.NoDelay = true;
        }
    }
}