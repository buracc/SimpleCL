using System.Collections.Concurrent;
using System.Collections.Generic;
using SimpleCL.SilkroadSecurityApi;

namespace SimpleCL.Interaction
{
    public static class InteractionQueue
    {
        public static readonly ConcurrentQueue<Packet> PacketQueue = new ConcurrentQueue<Packet>();
    }
}