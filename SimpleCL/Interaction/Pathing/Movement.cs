using SimpleCL.Enums.Commons;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.SecurityApi;
using SimpleCL.Util.Extension;

namespace SimpleCL.Interaction.Pathing
{
    public static class Movement
    {
        public static void WalkTo(LocalPoint localPoint)
        {
            WalkTo(localPoint.Region, localPoint.X, localPoint.Y, localPoint.Z);
        }

        public static void WalkTo(ushort region, float x, float y, float z)
        {
            if (LocalPlayer.Get.Tracing)
            {
                InteractionQueue.PacketQueue.Enqueue(new Packet(opcode: Opcode.Agent.Request.CHAR_ACTION, false, false,
                    new byte[] {0x02}));
            }

            var packet = new Packet(Opcode.Agent.Request.CHAR_MOVEMENT);
            packet.WriteByte(1);
            packet.WriteUShort(region);

            if (region > short.MaxValue)
            {
                packet.WriteInt(x);
                packet.WriteInt(z);
                packet.WriteInt(y);
            }
            else
            {
                packet.WriteUShort(x);
                packet.WriteUShort(z);
                packet.WriteUShort(y);
            }

            InteractionQueue.PacketQueue.Enqueue(packet);
        }
    }
}