using System;
using System.Linq;
using SimpleCL.Database;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Skills;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Entities.Exchange;
using SimpleCL.Models.Entities.Pet;
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

        [PacketHandler(Opcodes.Agent.Response.ENTITY_SOLO_SPAWN)]
        public void SingleEntitySpawn(Server server, Packet packet)
        {
            EntitySpawn(server, packet);
        }

        [PacketHandler(Opcodes.Agent.Response.ENTITY_SOLO_DESPAWN)]
        public void SingleEntityDespawn(Server server, Packet packet)
        {
            EntityDespawn(server, packet);
        }

        #endregion

        #region GroupSpawn

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
            QueryBuilder queryBuilder = null;

            if (_spawnCount > 0)
            {
                queryBuilder = new QueryBuilder(DirectoryUtils.GetDbFile(GameDatabase.Get.SelectedServer.Name + "_DB"),
                    true);
            }

            if (_spawnPacket != null)
            {
                _spawnPacket.Lock();
                _spawnCount.Repeat(i =>
                {
                    if (_spawnType == 1)
                    {
                        EntitySpawn(server, _spawnPacket, queryBuilder);
                    }
                    else
                    {
                        EntityDespawn(server, _spawnPacket);
                    }
                });
            }

            queryBuilder?.Finish();
        }

        #endregion

        #region OnTeleport

        [PacketHandler(Opcodes.Agent.Response.TELEPORT_READY)]
        public void TeleportUse(Server server, Packet packet)
        {
            Entities.Respawn();
            server.Inject(new Packet(Opcodes.Agent.Request.TELEPORT_READY));
        }

        #endregion

        #region Spawn Handlers

        private void EntityDespawn(Server server, Packet packet)
        {
            var uid = packet.ReadUInt();
            Entities.Despawned(uid);
        }

        private void EntitySpawn(Server server, Packet packet, QueryBuilder queryBuilder = null)
        {
            var refObjId = packet.ReadUInt();
            Entity entity;

            try
            {
                entity = Entity.FromId(refObjId, queryBuilder);
            }
            catch (EntityParseException e)
            {
                server.DebugPacket(packet);
                return;
            }

            switch (entity)
            {
                case SkillAoe skillAoe:
                {
                    try
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
                    }
                    catch (EntityParseException)
                    {
                        Console.WriteLine("failed to parse skillaoe");
                        server.DebugPacket(packet);
                        return;
                    }

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
                        if (packet.Opcode != Opcodes.Agent.Response.ENTITY_SOLO_SPAWN)
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
                            player.InventoryItems.Clear();
                            equipmentCount.Repeat(i =>
                            {
                                try
                                {
                                    var item = InventoryItem.FromId(packet.ReadUInt());
                                    player.InventoryItems.Add(item);
                                    if (item.IsEquipment())
                                    {
                                        var plus = packet.ReadByte();
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("failed to parse player equipment");
                                    server.DebugPacket(packet);
                                    throw;
                                }
                            });

                            var avatarInvSize = packet.ReadByte();
                            var avatarCount = packet.ReadByte();
                            avatarCount.Repeat(i =>
                            {
                                try
                                {
                                    var item = InventoryItem.FromId(packet.ReadUInt());
                                    player.InventoryItems.Add(item);
                                    if (item.IsEquipment())
                                    {
                                        var plus = packet.ReadByte();
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("failed to parse player avatars");
                                    server.DebugPacket(packet);
                                    throw;
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
                        try
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
                        }
                        catch (EntityParseException)
                        {
                            server.DebugPacket(packet);
                            throw;
                        }
                    });

                    switch (actor)
                    {
                        case Player p:
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

                            var transportFlag = packet.ReadByte();
                            var pvpState = packet.ReadByte();

                            if (transportFlag == 1)
                            {
                                var transportUid = packet.ReadUInt();
                            }

                            var scrollingType = packet.ReadByte();
                            p.InteractionType = (Player.Interaction) packet.ReadByte();

                            var guildName = packet.ReadAscii();

                            if (p.IsWearingJobSuit())
                            {
                                byte unks = 12;
                                unks.Repeat(i => { packet.ReadByte(); });
                            }
                            else
                            {
                                var guildId = packet.ReadUInt();
                                var grantName = packet.ReadAscii();

                                if (p.InteractionType == Player.Interaction.OnStall)
                                {
                                    byte unks = 14;
                                    unks.Repeat(i => { packet.ReadByte(); });

                                    var stallName = packet.ReadUnicode();
                                    p.Stall = new Stall
                                    {
                                        Title = stallName,
                                        PlayerUid = p.Uid
                                    };

                                    unks = 16;
                                    unks.Repeat(i => { packet.ReadByte(); });
                                }
                                else
                                {
                                    byte unks = 26;
                                    unks.Repeat(i => { packet.ReadByte(); });
                                }
                            }

                            break;
                        }

                        case Npc and Monster monster:
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
                                    if (!cos.IsHorse())
                                    {
                                        if (cos is AttackPet || cos is PickPet || cos is FellowPet)
                                        {
                                            cos.Name = packet.ReadAscii();
                                        }

                                        var owner = packet.ReadAscii();
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

                    if (packet.Opcode == Opcodes.Agent.Response.ENTITY_SOLO_SPAWN)
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