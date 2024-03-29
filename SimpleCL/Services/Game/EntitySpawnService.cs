﻿using System;
using System.Linq;
using SimpleCL.Database;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Items;
using SimpleCL.Enums.Skills;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Exchange;
using SimpleCL.Models.Entities.Npcs;
using SimpleCL.Models.Entities.Pets;
using SimpleCL.Models.Entities.Teleporters;
using SimpleCL.Models.Exceptions;
using SimpleCL.Models.Items;
using SimpleCL.Models.Skills;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Util;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game
{
    public class EntitySpawnService : Service
    {
        private Packet _spawnPacket;
        private byte _spawnType;
        private ushort _spawnCount;

        #region SingleSpawn

        [PacketHandler(Opcode.Agent.Response.ENTITY_SOLO_SPAWN)]
        public void SingleEntitySpawn(Server server, Packet packet)
        {
            EntitySpawn(server, packet);
        }

        [PacketHandler(Opcode.Agent.Response.ENTITY_SOLO_DESPAWN)]
        public void SingleEntityDespawn(Server server, Packet packet)
        {
            EntityDespawn(server, packet);
        }

        #endregion

        #region GroupSpawn

        [PacketHandler(Opcode.Agent.Response.ENTITY_GROUP_SPAWN_START)]
        public void GroupSpawnStart(Server server, Packet packet)
        {
            _spawnType = packet.ReadByte();
            _spawnCount = packet.ReadUShort();

            _spawnPacket = new Packet(Opcode.Agent.Response.ENTITY_GROUP_SPAWN_CHUNK);
        }

        [PacketHandler(Opcode.Agent.Response.ENTITY_GROUP_SPAWN_CHUNK)]
        public void GroupSpawnChunk(Server server, Packet packet)
        {
            _spawnPacket?.WriteByteArray(packet.GetBytes());
        }

        [PacketHandler(Opcode.Agent.Response.ENTITY_GROUP_SPAWN_END)]
        public void GroupSpawnEnd(Server server, Packet packet)
        {
            if (_spawnPacket == null)
            {
                return;
            }

            _spawnPacket.Lock();
            _spawnCount.Repeat(i =>
            {
                if (_spawnType == 1)
                {
                    EntitySpawn(server, _spawnPacket);
                }
                else
                {
                    EntityDespawn(server, _spawnPacket);
                }
            });
        }

        #endregion

        #region OnTeleport

        [PacketHandler(Opcode.Agent.Response.TELEPORT_READY)]
        public void TeleportUse(Server server, Packet packet)
        {
            Entities.Respawn();
            server.Inject(new Packet(Opcode.Agent.Request.TELEPORT_READY));
        }

        #endregion

        #region Spawn Handlers

        private void EntityDespawn(Server server, Packet packet)
        {
            var uid = packet.ReadUInt();
            Entities.Despawned(uid);
        }

        private void EntitySpawn(Server server, Packet packet)
        {
            var refObjId = packet.ReadUInt();
            Entity entity;

            try
            {
                entity = Entity.FromId(refObjId);
            }
            catch (EntityParseException e)
            {
                Console.WriteLine(e);
                server.DebugPacket(packet);
                throw;
            }

            switch (entity)
            {
                case SkillAoe skillAoe:
                {
                    packet.ReadUInt();
                    var skillId = packet.ReadUInt();
                    skillAoe.Skill = new Skill(skillId);
                    skillAoe.Uid = packet.ReadUInt();
                    skillAoe.LocalPoint = new LocalPoint(
                        packet.ReadUShort(),
                        packet.ReadFloat(),
                        packet.ReadFloat(),
                        packet.ReadFloat()
                    );
                    var angle = packet.ReadUShort();
                    break;
                }

                case Teleport teleport:
                {
                    teleport.Uid = packet.ReadUInt();
                    teleport.LocalPoint = new LocalPoint(
                        packet.ReadUShort(),
                        packet.ReadFloat(),
                        packet.ReadFloat(),
                        packet.ReadFloat()
                    );

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

                    if (unk != 1)
                    {
                        break;
                    }

                    packet.ReadByte();
                    packet.ReadByte();

                    break;
                }

                case GroundItem groundItem:
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

                        groundItem.Uid = packet.ReadUInt();
                        groundItem.LocalPoint = new LocalPoint(
                            packet.ReadUShort(),
                            packet.ReadFloat(),
                            packet.ReadFloat(),
                            packet.ReadFloat()
                        );

                        var angle = packet.ReadUShort();
                        var hasOwner = packet.ReadByte() == 1;
                        if (hasOwner)
                        {
                            var ownerJid = packet.ReadUInt();
                        }

                        var rarity = packet.ReadByte();
                        if (packet.Opcode != Opcode.Agent.Response.ENTITY_SOLO_SPAWN)
                        {
                            break;
                        }

                        var dropSourceType = packet.ReadByte();
                        var dropUid = packet.ReadUInt();
                    }

                    break;
                }

                case Actor actor:
                {
                    switch (actor)
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
                            player.Inventory.Clear();
                            equipmentCount.Repeat(i =>
                            {
                                var item = InventoryItem.FromId(packet.ReadUInt());
                                player.Inventory.Add(item);
                                if (item.Category == ItemCategory.Equipment)
                                {
                                    var plus = packet.ReadByte();
                                }
                            });

                            var avatarInvSize = packet.ReadByte();
                            var avatarCount = packet.ReadByte();
                            avatarCount.Repeat(i =>
                            {
                                var item = InventoryItem.FromId(packet.ReadUInt());
                                player.Inventory.Add(item);
                                if (item.Category == ItemCategory.Equipment)
                                {
                                    var plus = packet.ReadByte();
                                }
                            });

                            var hasMask = packet.ReadByte() == 1;
                            if (hasMask)
                            {
                                var mask = new Mask(packet.ReadUInt());
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

                    actor.Uid = packet.ReadUInt();

                    actor.LocalPoint = new LocalPoint(
                        packet.ReadUShort(),
                        packet.ReadFloat(),
                        packet.ReadFloat(),
                        packet.ReadFloat()
                    );

                    actor.Angle = packet.ReadUShort();

                    var destinationSet = packet.ReadByte() == 1;
                    actor.WalkMode = (Actor.Movement.Mode) packet.ReadByte();

                    if (destinationSet)
                    {
                        var destinationRegion = packet.ReadUShort();
                        var inDungeon = actor.LocalPoint.Region > short.MaxValue;
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
                        actor.Angle = packet.ReadUShort();
                    }

                    actor.LifeState = (Actor.Health.LifeState) packet.ReadByte();

                    packet.ReadByte();
                    actor.Motion = (Actor.Movement.Motion) packet.ReadByte();
                    var status = packet.ReadByte();

                    packet.ReadByte(); // idk what position, but there is an unknown byte before walkspeed

                    actor.WalkSpeed = packet.ReadFloat();
                    actor.RunSpeed = packet.ReadFloat();
                    actor.ZerkSpeed = packet.ReadFloat();

                    var buffCount = packet.ReadByte();
                    buffCount.Repeat(i =>
                    {
                        var refSkillId = packet.ReadUInt();
                        var buff = new Buff(refSkillId);

                        if (buff.IsBardAreaBuff())
                        {
                            buff.Uid = packet.ReadUInt();
                            var isBuffOwner = packet.ReadBool();
                        }
                        else
                        {
                            buff.RemainingDuration = packet.ReadUInt();
                        }

                        if (buff.IsRecoveryDivision())
                        {
                            var isBuffOwner = packet.ReadByte();
                            if (isBuffOwner == 1)
                            {
                                buff.CasterUid = actor.Uid;
                            }
                        }

                        buff.TargetUid = actor.Uid;
                        actor.Buffs.Add(buff);
                    });

                    switch (actor)
                    {
                        case Player p:
                        {
                            p.Name = packet.ReadAscii();
                            var inCombat = packet.ReadByte();

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

                            var transportFlag = packet.ReadByte();
                            var pvpState = packet.ReadByte();

                            if (transportFlag == 1)
                            {
                                var transportUid = packet.ReadUInt();
                            }

                            var scrollingType = packet.ReadByte();
                            p.InteractionType = (Player.Interaction) packet.ReadByte();

                            var guildName = packet.ReadAscii();

                            if (!p.IsWearingJobSuit())
                            {
                                var guildId = packet.ReadUInt();
                                var grantName = packet.ReadAscii();
                                var lastCrestRev = packet.ReadUInt();
                                var unionId = packet.ReadUInt();
                                var unionLastCrestRev = packet.ReadUInt();
                                var friendly = packet.ReadBool();
                                var siegeAuthority = packet.ReadByte();
                            }
                            
                            switch (p.InteractionType)
                            {
                                case Player.Interaction.OnStall:
                                {
                                    var stallName = packet.ReadUnicode();
                                    p.Stall = new Stall
                                    {
                                        Title = stallName,
                                        PlayerUid = p.Uid
                                    };
                                    var decorationItemId = packet.ReadUInt();
                                    break;
                                }
                                case Player.Interaction.OnExchange:
                                    break;
                            }

                            packet.ReadUShort();
                            var zerkSkinItemId = packet.ReadUInt();
                            if (zerkSkinItemId > 0)
                            {
                                var timeLeft = packet.ReadUInt();
                            }

                            packet.ReadUInt();
                            packet.ReadUShort();
                            break;
                        }

                        case Monster monster:
                        {
                            packet.ReadByte();
                            packet.ReadByte();
                            var unkByteAmount = packet.ReadByte();
                            var unkBytes = packet.ReadByteArray(unkByteAmount);
                            var mobType = (Monster.Type) unkBytes[0];
                            break;
                        }

                        case Npc npc:
                        {
                            var talk = packet.ReadByte();
                            var hasTalk = talk == 2;
                            if (hasTalk)
                            {
                                var amount = packet.ReadByte();
                                var talkOptions = packet.ReadByteArray(amount);
                            }

                            switch (npc)
                            {
                                case TalkNpc talkNpc:
                                    break;

                                case Cos cos:
                                {
                                    if (cos is not Horse)
                                    {
                                        if (cos is AttackPet || cos is PickPet || cos is FellowPet)
                                        {
                                            cos.Name = packet.ReadAscii();
                                        }

                                        var owner = packet.ReadAscii();
                                        if (cos is QuestPet)
                                        {
                                            packet.ReadUShort();
                                            packet.ReadUShort();
                                            packet.ReadByte();
                                            break;
                                        }

                                        var jobType = packet.ReadByte();
                                        if (cos is not PickPet)
                                        {
                                            var pvpState = packet.ReadByte();
                                        }

                                        if (cos is GuildGuard)
                                        {
                                            var ownerObjId = packet.ReadUInt();
                                        }

                                        cos.OwnerUid = packet.ReadUInt();

                                        if (cos is FellowPet)
                                        {
                                            packet.ReadByte();
                                        }
                                    }

                                    break;
                                }

                                case FortressCos fortressCos:
                                {
                                    var guildId = packet.ReadUInt();
                                    var guildName = packet.ReadAscii();
                                    break;
                                }
                            }

                            break;
                        }
                    }

                    if (packet.Opcode == Opcode.Agent.Response.ENTITY_SOLO_SPAWN)
                    {
                        packet.ReadByte();
                    }

                    break;
                }
            }

            Entities.Spawned(entity);
        }

        #endregion
    }
}