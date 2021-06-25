using System;
using System.IO;
using System.Linq;
using SilkroadSecurityApi;
using SimpleCL.Database;
using SimpleCL.Enums.Common;
using SimpleCL.Enums.Server;
using SimpleCL.Enums.Skill;
using SimpleCL.Model.Character;
using SimpleCL.Model.Coord;
using SimpleCL.Model.Entity;
using SimpleCL.Model.Inventory;
using SimpleCL.Network;
using SimpleCL.Util.Extension;

namespace SimpleCL.Service.Game.Entity
{
    public class SpawnService : Service
    {
        private readonly SilkroadServer _silkroadServer;

        private Packet _spawnPacket;
        private byte _spawnType;
        private ushort _spawnCount;

        public SpawnService(SilkroadServer silkroadServer)
        {
            _silkroadServer = silkroadServer;
        }

        [PacketHandler(Opcodes.Agent.Response.ENTITY_SOLO_SPAWN)]
        public void SingleEntitySpawn(Server server, Packet packet)
        {
            EntitySpawn(server, packet);
        }

        [PacketHandler(Opcodes.Agent.Response.ENTITY_GROUP_SPAWN_START)]
        public void GroupSpawnStart(Server server, Packet packet)
        {
            _spawnType = packet.ReadByte();
            _spawnCount = packet.ReadUShort();

            _spawnPacket = new Packet(Opcodes.Agent.Response.ENTITY_GROUP_SPAWN_CHUNK);
        }

        [PacketHandler(Opcodes.Agent.Response.ENTITY_GROUP_SPAWN_CHUNK)]
        public void GroupSpawnChunk(Server server, Packet packet)
        {
            _spawnPacket?.WriteByteArray(packet.GetBytes());
        }

        [PacketHandler(Opcodes.Agent.Response.ENTITY_GROUP_SPAWN_END)]
        public void GroupSpawnEnd(Server server, Packet packet)
        {
            if (_spawnPacket != null)
            {
                _spawnPacket.Lock();

                if (_spawnType == 1)
                {
                    EntitySpawn(server, _spawnPacket);
                }

                // _spawnCount.Repeat(i =>
                // {
                //     
                // });
            }
        }

        private void EntitySpawn(Server server, Packet packet)
        {
            try
            {
                var refObjId = packet.ReadUInt();
                var entity = Model.Entity.Entity.FromId(refObjId);

                if (entity is SkillAoe skillAoe)
                {
                    Console.WriteLine("Skill AoE spawned");
                    Console.WriteLine(skillAoe.Name);
                    packet.ReadUShort();
                    var skillId = packet.ReadUInt();
                    var uid = packet.ReadUInt();
                    var position = new LocalPoint(
                        packet.ReadUShort(),
                        packet.ReadFloat(),
                        packet.ReadFloat(),
                        packet.ReadFloat()
                    );
                    Console.WriteLine(position);
                    var angle = packet.ReadUShort();
                    return;
                }

                if (entity is Teleport teleport)
                {
                    Console.WriteLine("Teleport spawned");
                    Console.WriteLine(teleport.Name);
                    var uid = packet.ReadUInt();
                    var position = new LocalPoint(
                        packet.ReadUShort(),
                        packet.ReadFloat(),
                        packet.ReadFloat(),
                        packet.ReadFloat()
                    );

                    Console.WriteLine(position);

                    var angle = packet.ReadUShort();
                    packet.ReadByte();
                    var unk = packet.ReadByte();
                    packet.ReadByte();
                    var type = (Teleport.Type) packet.ReadByte();
                    switch (type)
                    {
                        case Teleport.Type.Regular:
                            packet.ReadUInt();
                            packet.ReadUInt();
                            break;

                        case Teleport.Type.Dimensional:
                        {
                            var owner = packet.ReadAscii();
                            var ownerUid = packet.ReadUInt();
                            break;
                        }
                    }

                    if (unk == 1)
                    {
                        packet.ReadByte();
                        packet.ReadByte();
                    }

                    return;
                }

                if (entity is GroundItem groundItem)
                {
                    Console.WriteLine("Grounditem spawned");
                    Console.WriteLine(groundItem.Name);
                    if (groundItem.IsEquipment())
                    {
                        var plus = packet.ReadByte();
                    }
                    else if (groundItem.IsConsumable())
                    {
                        if (groundItem.IsGold())
                        {
                            var gold = packet.ReadUInt();
                        }
                        else if (groundItem.IsQuest() || groundItem.IsTradeGoods())
                        {
                            var owner = packet.ReadAscii();
                        }

                        var uid = packet.ReadUInt();
                        var position = new LocalPoint(
                            packet.ReadUShort(),
                            packet.ReadFloat(),
                            packet.ReadFloat(),
                            packet.ReadFloat()
                        );
                        Console.WriteLine(position);
                        var angle = packet.ReadUShort();
                        var hasOwner = packet.ReadByte() == 1;
                        if (hasOwner)
                        {
                            var ownerJid = packet.ReadUInt();
                        }

                        var rarity = packet.ReadByte();
                        if (packet.Opcode == Opcodes.Agent.Response.ENTITY_SOLO_SPAWN)
                        {
                            var dropSourceType = packet.ReadByte();
                            var dropUid = packet.ReadUInt();
                        }
                    }

                    return;
                }

                if (entity is PathingEntity pathingEntity)
                {
                    switch (pathingEntity)
                    {
                        case Player player:
                        {
                            Console.WriteLine("Player spawned");
                            var scale = packet.ReadByte();
                            var zerkLevel = packet.ReadByte();
                            var pvpCapeType = packet.ReadByte();
                            packet.ReadByte();
                            var expIconType = packet.ReadByte();

                            var invSize = packet.ReadByte();
                            var equipmentCount = packet.ReadByte();
                            player.InventoryItems.Clear();
                            equipmentCount.Repeat(i =>
                            {
                                InventoryItem item = InventoryItem.FromId(packet.ReadUInt());
                                player.InventoryItems.Add(item);
                                if (item.IsEquipment())
                                {
                                    var plus = packet.ReadByte();
                                }
                            });

                            var avatarInvSize = packet.ReadByte();
                            var avatarCount = packet.ReadByte();
                            avatarCount.Repeat(i =>
                            {
                                InventoryItem item = InventoryItem.FromId(packet.ReadUInt());
                                if (item.IsEquipment())
                                {
                                    var plus = packet.ReadByte();
                                }
                            });

                            var hasMask = packet.ReadByte() == 1;
                            if (hasMask)
                            {
                                Mask mask = new Mask(packet.ReadUInt());
                                if (mask.TypeId1 == player.TypeId1 && mask.TypeId2 == player.TypeId2)
                                {
                                    var maskScale = packet.ReadByte();
                                    var maskInventory = packet.ReadUIntArray(packet.ReadByte());
                                }
                            }

                            break;
                        }
                        case FortressStructure structure:
                        {
                            var hp = packet.ReadUInt();
                            var refEventStructId = packet.ReadUInt();
                            var state = packet.ReadUShort();
                            break;
                        }
                    }

                    var uid = packet.ReadUInt();
                    var localPoint = new LocalPoint(
                        packet.ReadUShort(),
                        packet.ReadFloat(),
                        packet.ReadFloat(),
                        packet.ReadFloat()
                    );

                    Console.WriteLine(localPoint);
                    var angle = packet.ReadUShort();

                    var destinationSet = packet.ReadByte() == 1;
                    var walkType = packet.ReadByte();

                    if (destinationSet)
                    {
                        var destinationRegion = packet.ReadUShort();
                        var inDungeon = localPoint.Region > short.MaxValue;
                        if (inDungeon)
                        {
                            var destinationXOffset = packet.ReadInt();
                            var destinationZOffset = packet.ReadInt();
                            var destinationYOffset = packet.ReadInt();
                        }
                        else
                        {
                            var destinationXOffset = packet.ReadUShort();
                            var destinationZOffset = packet.ReadUShort();
                            var destinationYOffset = packet.ReadUShort();
                        }
                    }
                    else
                    {
                        var movementType = packet.ReadByte();
                        angle = packet.ReadUShort();
                    }

                    var lifeState = packet.ReadByte();

                    packet.ReadByte();
                    var motionState = packet.ReadByte();
                    var status = packet.ReadByte();

                    packet.ReadByte(); // idk what position, but there is an unknown byte before walkspeed

                    var walkSpeed = packet.ReadFloat();
                    var runSpeed = packet.ReadFloat();
                    var zerkSpeed = packet.ReadFloat();

                    var buffCount = packet.ReadByte();
                    buffCount.Repeat(i =>
                    {
                        var refSkillId = packet.ReadUInt();
                        var duration = packet.ReadUInt();
                        var skillData = GameDatabase.Get.GetSkill(refSkillId);
                        var autoTransferEffect = skillData["attributes"]
                            .Split(',')
                            .Contains(BuffData.Attribute.AutoTransferEffect.ToString());

                        if (autoTransferEffect)
                        {
                            var isBuffOwner = packet.ReadByte();
                        }
                    });

                    if (pathingEntity is Player p)
                    {
                        p.Name = packet.ReadAscii();

                        var inCombat = packet.ReadByte();

                        // no clue how this works but theres 3 extra bytes without any boolean flag
                        // if player is in job mode
                        if (p.IsWearingJobSuit())
                        {
                            packet.ReadByte();
                            var level = packet.ReadByte();
                            packet.ReadByte();
                        }

                        var transportFlag = packet.ReadByte();
                        var pvpState = packet.ReadByte();

                        if (transportFlag == 1)
                        {
                            var transportUid = packet.ReadUInt();
                        }

                        var scrollingType = packet.ReadByte();
                        var interactionType = packet.ReadByte();

                        var guildName = packet.ReadAscii();
                    }
                    else if (pathingEntity is Npc npc)
                    {
                        //todo
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}