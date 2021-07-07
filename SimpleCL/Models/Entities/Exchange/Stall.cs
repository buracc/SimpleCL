using System.Collections.Generic;
using System.ComponentModel;
using SimpleCL.Enums.Commons;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Entities.Exchange
{
    public class Stall
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ulong PlayerUid { get; set; }
        public bool Opened { get; set; }
        public readonly BindingList<StallItem> Items = new();

        public void Visit()
        {
            var openPacket = new Packet(Opcodes.Agent.Request.STALL_TALK);
            openPacket.WriteUInt(PlayerUid);
            Interaction.InteractionQueue.PacketQueue.Enqueue(openPacket);
        }


        public void Leave()
        {
            var exitPacket = new Packet(Opcodes.Agent.Request.STALL_LEAVE);
            Interaction.InteractionQueue.PacketQueue.Enqueue(exitPacket);
        }
    }
}