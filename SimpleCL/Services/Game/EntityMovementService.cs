using System;
using SimpleCL.Enums.Commons;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
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
            var uid = packet.ReadUInt();
            var destinationSet = packet.ReadByte() == 1;
            if (!destinationSet)
            {
                return;
            }

            var region = packet.ReadUShort();
            LocalPoint destination;
            if (region > short.MaxValue)
            {
                destination = new LocalPoint(
                    region,
                    packet.ReadInt(),
                    packet.ReadInt(),
                    packet.ReadInt()
                );
            }
            else
            {
                destination = new LocalPoint(
                    region,
                    packet.ReadUShort(),
                    packet.ReadUShort(),
                    packet.ReadUShort()
                );
            }

            if (!Entities.AllEntities.TryGetValue(uid, out var entity) || entity is not Actor actor)
            {
                return;
            }

            ushort angle = 0;

            var oldWorldPos = actor.WorldPoint;
            var newWorldPos = WorldPoint.FromLocal(destination);
            
            var xDiff = newWorldPos.X - oldWorldPos.X;
            var yDiff = newWorldPos.Y - oldWorldPos.Y;
            
            if (xDiff == 0)
            {
                angle = (ushort) (ushort.MaxValue / 4 * (yDiff > 0 ? 1 : 3));
            }
            else
            {
                if (yDiff == 0)
                {
                    angle = (ushort) (ushort.MaxValue / 4 * (yDiff > 0 ? 1 : 3));
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

                    angle =
                        (ushort) Math.Round(angleRadians * ushort.MaxValue / (Math.PI * 2.0));
                }
            }
            
            Entities.Moved(uid, destination, angle);
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