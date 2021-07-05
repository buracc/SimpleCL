using System;
using SimpleCL.Enums.Commons;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game
{
    public class EntityMovementService : Service
    {
        #region OnMovement

        [PacketHandler(Opcodes.Agent.Response.ENTITY_MOVEMENT)]
        public void EntityMovement(Server server, Packet packet)
        {
            var oldPos = LocalPlayer.Get.WorldPoint;
            
            try
            {
                var uid = packet.ReadUInt();
                var destinationSet = packet.ReadByte() == 1;
                if (!destinationSet)
                {
                    return;
                }
                
                var region = packet.ReadUShort();
                LocalPoint localPoint;
                if (region > short.MaxValue)
                {
                    localPoint = new LocalPoint(
                        region,
                        packet.ReadInt(),
                        packet.ReadInt(),
                        packet.ReadInt()
                    );
                }
                else
                {
                    localPoint = new LocalPoint(
                        region,
                        packet.ReadUShort(),
                        packet.ReadUShort(),
                        packet.ReadUShort()
                    );
                }
                    
                Entities.Moved(uid, localPoint);

                if (uid != LocalPlayer.Get.Uid)
                {
                    return;
                }
                
                var xDiff = LocalPlayer.Get.WorldPoint.X - oldPos.X;
                var yDiff = LocalPlayer.Get.WorldPoint.Y - oldPos.Y;
                if (xDiff == 0)
                {
                    LocalPlayer.Get.Angle = (ushort) (ushort.MaxValue / 4 * (yDiff > 0 ? 1 : 3));
                }
                else
                {
                    if (yDiff == 0)
                    {
                        LocalPlayer.Get.Angle = (ushort) (ushort.MaxValue / 4 * (yDiff > 0 ? 1 : 3));
                    }
                    else
                    {
                        var angleRadians = Math.Atan(yDiff / xDiff);

                        if (yDiff < 0 || xDiff < 0)
                        {
                            angleRadians += Math.PI;
                            if (xDiff > 0)
                            {
                                angleRadians += Math.PI;
                            }
                        }

                        LocalPlayer.Get.Angle =
                            (ushort) Math.Round(angleRadians * ushort.MaxValue / (Math.PI * 2.0));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to parse movement packet");
                server.DebugPacket(packet);
                Console.WriteLine(e);
            }
        }

        #endregion

        #region MovementHalted

        [PacketHandler(Opcodes.Agent.Response.ENTITY_MOVEMENT_HALT)]
        public void EntityHalt(Server server, Packet packet)
        {
            var uid = packet.ReadUInt();
            var stuckPosition = new LocalPoint(
                packet.ReadUShort(),
                packet.ReadFloat(),
                packet.ReadFloat(),
                packet.ReadFloat()
            );
            var angle = packet.ReadUShort();

            Entities.Moved(uid, stuckPosition, angle);
        }

        #endregion

        #region AngleChanged

        [PacketHandler(Opcodes.Agent.Response.ENTITY_MOVEMENT_ANGLE)]
        public void EntityAngle(Server server, Packet packet)
        {
            Entities.Moved(packet.ReadUInt(), angle: packet.ReadUShort());
        }

        #endregion

        #region SpeedChanged

        [PacketHandler(Opcodes.Agent.Response.ENTITY_SPEED)]
        public void EntitySpeed(Server server, Packet packet)
        {
            Entities.SpeedChanged(
                packet.ReadUInt(),
                packet.ReadFloat(),
                packet.ReadFloat()
            );
        }

        #endregion
    }
}