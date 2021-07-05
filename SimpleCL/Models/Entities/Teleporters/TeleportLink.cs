using SimpleCL.Enums.Commons;
using SimpleCL.Interaction;
using SimpleCL.SecurityApi;
using SimpleCL.Util.Extension;

namespace SimpleCL.Models.Entities.Teleporters
{
    public class TeleportLink
    {
        public readonly uint DestinationId;
        public readonly string Name;

        public TeleportLink(uint destinationId, string name)
        {
            DestinationId = destinationId;
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Teleport(Teleport teleporter)
        {
            var teleportPacket = new Packet(Opcodes.Agent.Request.TELEPORT_USE);
            teleportPacket.WriteUInt(teleporter.Uid);
            teleportPacket.WriteByte(2);
            teleportPacket.WriteUInt(DestinationId);
            InteractionQueue.PacketQueue.Enqueue(teleportPacket);
        }
    }
}