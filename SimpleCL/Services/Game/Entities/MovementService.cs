using System;
using SilkroadSecurityApi;
using SimpleCL.Enums.Commons;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.Network;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game.Entities
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
                        var oldPos = LocalPlayer.Get.WorldPoint;
                        
                        LocalPlayer.Get.LocalPoint = localPoint;
                        
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
                                double angleRadians = Math.Atan(yDiff / xDiff);

                                if(yDiff < 0 || xDiff < 0)
                                {
                                    angleRadians += Math.PI;
                                    if (xDiff > 0)
                                    {
                                        angleRadians += Math.PI;
                                    }
                                }

                                LocalPlayer.Get.Angle = (ushort)Math.Round(angleRadians * ushort.MaxValue / (Math.PI * 2.0));
                            }
                        }
                    }
                    
                    Interaction.Providers.Entities.Moved(uid, localPoint);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to parse movement packet");
                server.DebugPacket(packet);
                Console.WriteLine(e);
            }
        }
    }
}