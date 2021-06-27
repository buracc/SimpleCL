﻿using System;
using System.Linq;
using SilkroadSecurityApi;
using SimpleCL.Database;
using SimpleCL.Enums.Common;
using SimpleCL.Enums.Skill;
using SimpleCL.Model.Coord;
using SimpleCL.Model.Entity;
using SimpleCL.Model.Entity.Mob;
using SimpleCL.Model.Entity.Pet;
using SimpleCL.Model.Inventory;
using SimpleCL.Network;
using SimpleCL.Util.Extension;

namespace SimpleCL.Service.Game.Entity
{
    public class SpawnService : Service
    {
        private Packet _spawnPacket;
        private byte _spawnType;
        private ushort _spawnCount;

        [PacketHandler(Opcodes.Agent.Response.ENTITY_SOLO_SPAWN)]
        public void SingleEntitySpawn(Server server, Packet packet)
        {
            // server.DebugPacket(packet);
            // Console.WriteLine();
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
                // server.DebugPacket(_spawnPacket);
                // Console.WriteLine();
                _spawnPacket.Lock();
                _spawnCount.Repeat(i =>
                {
                    if (_spawnType == 1)
                    {
                        EntitySpawn(server, _spawnPacket);
                    }
                });
            }
        }

        private void EntitySpawn(Server server, Packet packet)
        {
            try
            {
                // Console.WriteLine("------------");
                var refObjId = packet.ReadUInt();
                var entity = Model.Entity.Entity.FromId(refObjId);
                // Console.WriteLine(entity.ToString());
                
                if (entity is SkillAoe skillAoe)
                {
                    var skillId = packet.ReadUInt();
                    // Console.WriteLine(skillId);
                    var skillData = GameDatabase.Get.GetSkill(skillId);
                    // Console.WriteLine(skillData["name"]);
                    var uid = packet.ReadUInt();
                    skillAoe.LocalPoint = new LocalPoint(
                        packet.ReadUShort(),
                        packet.ReadFloat(),
                        packet.ReadFloat(),
                        packet.ReadFloat()
                    );
                    var angle = packet.ReadUShort();
                    // Program.Gui.AddMinimapEntity(uid, skillAoe);
                    return;
                }

                if (entity is Teleport teleport)
                {
                    // Console.WriteLine(teleport.Name);
                    var uid = packet.ReadUInt();
                    teleport.LocalPoint = new LocalPoint(
                        packet.ReadUShort(),
                        packet.ReadFloat(),
                        packet.ReadFloat(),
                        packet.ReadFloat()
                    );
                    Program.Gui.AddMinimapEntity(uid, teleport);

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
                        groundItem.LocalPoint = new LocalPoint(
                            packet.ReadUShort(),
                            packet.ReadFloat(),
                            packet.ReadFloat(),
                            packet.ReadFloat()
                        );
                        Program.Gui.AddMinimapEntity(uid, groundItem);
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
                    
                    // Console.WriteLine("uid: " + uid);
                    pathingEntity.LocalPoint = new LocalPoint(
                        packet.ReadUShort(),
                        packet.ReadFloat(),
                        packet.ReadFloat(),
                        packet.ReadFloat()
                    );
                    
                    Program.Gui.AddMinimapEntity(uid, pathingEntity);

                    var angle = packet.ReadUShort();

                    var destinationSet = packet.ReadByte() == 1;
                    var walkType = packet.ReadByte();

                    if (destinationSet)
                    {
                        var destinationRegion = packet.ReadUShort();
                        var inDungeon = pathingEntity.LocalPoint.Region > short.MaxValue;
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
                            packet.ReadByte();
                            packet.ReadByte();
                        }

                        if (p.IsWearingJobSuit())
                        {
                            p.Name = "*" + p.Name;
                        }

                        // Console.WriteLine("player name: " + p.Name);

                        var transportFlag = packet.ReadByte();
                        var pvpState = packet.ReadByte();

                        if (transportFlag == 1)
                        {
                            var transportUid = packet.ReadUInt();
                        }

                        var scrollingType = packet.ReadByte();
                        var interactionType = packet.ReadByte();

                        var guildName = packet.ReadAscii();
                        // Console.WriteLine(guildName);

                        if (p.IsWearingJobSuit())
                        {
                            byte unks = 12;
                            unks.Repeat(i => { packet.ReadByte(); });
                        }
                        else
                        {
                            var guildId = packet.ReadUInt();
                            var grantName = packet.ReadAscii();

                            if (interactionType == 3)
                            {
                                byte unks = 14;
                                unks.Repeat(i => { packet.ReadByte(); });

                                var stallName = packet.ReadUnicode();
                                // Console.WriteLine("player is stalling: " + stallName);

                                unks = 16;
                                unks.Repeat(i => { packet.ReadByte(); });
                            }
                            else
                            {
                                byte unks = 26;
                                unks.Repeat(i => { packet.ReadByte(); });
                            }
                        }
                    }
                    else if (pathingEntity is Npc npc)
                    {
                        if (npc is Monster monster)
                        {
                            packet.ReadByte();
                            packet.ReadByte();
                            var unkByteAmount = packet.ReadByte();
                            var unkBytes = packet.ReadByteArray(unkByteAmount);
                            var mobType = (Monster.Type) unkBytes[0];
                            // Console.WriteLine("monster type: " + mobType);
                        }
                        else
                        {
                            var talk = packet.ReadByte();
                            var hasTalk = talk == 2;
                            if (hasTalk)
                            {
                                var amount = packet.ReadByte();
                                // Console.WriteLine("npc has " + amount + " talkoptions");
                                var talkOptions = packet.ReadByteArray(amount);
                            }
                            
                            if (npc is Cos cos)
                            {
                                if (!cos.IsHorse())
                                {
                                    if (cos is AttackPet || cos is PickPet || cos is FellowPet)
                                    {
                                        cos.Name = packet.ReadAscii();
                                        // Console.WriteLine("cos name: " + cos.Name);
                                    }

                                    var owner = packet.ReadAscii();
                                    // Console.WriteLine("cos owner: " + owner);
                                    var jobType = packet.ReadByte();

                                    if (!cos.IsPickPet())
                                    {
                                        var pvpState = packet.ReadByte();
                                    }

                                    if (cos.IsGuildGuard())
                                    {
                                        var ownerObjId = packet.ReadUInt();
                                    }

                                    var ownerUid = packet.ReadUInt();
                                    // Console.WriteLine("cos owner uid: " + ownerUid);
                                }
                            }
                            else if (npc is FortressCos fortressCos)
                            {
                                var guildId = packet.ReadUInt();
                                var guildName = packet.ReadAscii();
                            }
                        }
                    }

                    if (packet.Opcode == Opcodes.Agent.Response.ENTITY_SOLO_SPAWN)
                    {
                        packet.ReadByte();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
    }
}