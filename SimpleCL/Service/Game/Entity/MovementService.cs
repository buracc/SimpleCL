using System;
using SilkroadSecurityApi;
using SimpleCL.Enums.Common;
using SimpleCL.Model.Character;
using SimpleCL.Model.Coord;
using SimpleCL.Network;
using SimpleCL.Util.Extension;

namespace SimpleCL.Service.Game.Entity
{
    public class MovementService : Service
    {
        [PacketHandler(Opcodes.Agent.Response.ENTITY_MOVEMENT)]
        public void EntityMovement(Server server, Packet packet)
        {
            try
            {
                var uid = packet.ReadUInt();
                var destinationSet = packet.ReadByte() == 1;
                if (destinationSet)
                {
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

                    if (uid == LocalPlayer.Get.Uid)
                    {
                        LocalPlayer.Get.LocalPoint = localPoint;
                        Console.WriteLine("we are moving: " + localPoint);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Failed to parse movement packet");
                server.DebugPacket(packet);
            }
        }
    }
}