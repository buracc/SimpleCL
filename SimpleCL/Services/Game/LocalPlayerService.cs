using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Quests;
using SimpleCL.Enums.Server;
using SimpleCL.Enums.Skills;
using SimpleCL.Interaction.Providers;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Items;
using SimpleCL.Models.Skills;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game
{
    public class LocalPlayerService : Service
    {
        private readonly SilkroadServer _silkroadServer;
        private readonly Gateway _gateway;
        private readonly LocalPlayer _localPlayer = LocalPlayer.Get;

        public LocalPlayerService(SilkroadServer silkroadServer, Gateway gateway)
        {
            _silkroadServer = silkroadServer;
            _gateway = gateway;
        }

        #region Spawned

        [PacketHandler(Opcodes.Agent.Response.CHAR_DATA_CHUNK)]
        public void GameJoined(Server server, Packet packet)
        {
            _gateway?.Dispose();
            Program.Gui.ClearMarkers();
            Program.Gui.ClearTiles();

            var serverTime = packet.ReadUInt();
            var refObjId = packet.ReadUInt();
            var scale = packet.ReadByte();
            _localPlayer.Level = packet.ReadByte();
            var maxLevel = packet.ReadByte();
            _localPlayer.ExpGained = packet.ReadULong();
            var spOffset = packet.ReadUInt();
            _localPlayer.Gold = packet.ReadULong();
            _localPlayer.Skillpoints = packet.ReadUInt();
            var statPoint = packet.ReadUShort();
            var zerkPoints = packet.ReadByte();
            var gatheredExp = packet.ReadUInt(); // GatheredExp according to DaxterSoul, but it's wrong on TRSRO
            _localPlayer.MaxHp = packet.ReadUInt();
            _localPlayer.MaxMp = packet.ReadUInt();
            var icon = packet.ReadByte();
            var dailyPk = packet.ReadByte();
            var totalPk = packet.ReadUShort();
            var pkPoint = packet.ReadUInt();
            var zerkLevel = packet.ReadByte();
            var freePvp = packet.ReadByte();

            if (_silkroadServer.Locale.IsInternational())
            {
                byte unknownBytes = 25;
                unknownBytes.Repeat(i => { packet.ReadByte(); });
            }

            var inventorySize = packet.ReadByte();
            var itemCount = packet.ReadByte();

            var inv = ParseInventory(packet, itemCount, _silkroadServer.Locale);

            foreach (var inventoryItem in inv.Where(x => x.Slot > 12))
            {
                _localPlayer.Inventory.Add(inventoryItem);
            }

            foreach (var equipment in inv.Where(x => x.Slot < 13))
            {
                _localPlayer.EquipmentInventory.Add(equipment);
            }

            var avatarInventorySize = packet.ReadByte();
            var avatarInventoryCount = packet.ReadByte();

            var avatars = ParseInventory(packet, avatarInventoryCount, _silkroadServer.Locale, false);
            foreach (var avatar in avatars)
            {
                _localPlayer.AvatarInventory.Add(avatar);
            }

            if (_silkroadServer.Locale.IsInternational())
            {
                var jobPouchSize = packet.ReadByte();
                if (jobPouchSize > 0)
                {
                    var jobPouchCount = packet.ReadByte();
                    jobPouchCount.Repeat(j =>
                    {
                        var slot = packet.ReadByte();
                        packet.ReadUInt();
                        var itemId = packet.ReadUInt();
                        var stackSize = packet.ReadUInt();
                    });
                }

                var jobInventorySize = packet.ReadByte();
                var jobInventoryCount = packet.ReadByte();

                var jobEquipment = ParseInventory(packet, jobInventoryCount, _silkroadServer.Locale, false);
                foreach (var jobEquip in jobEquipment)
                {
                    _localPlayer.JobEquipmentInventory.Add(jobEquip);
                }
            }

            packet.ReadByte();

            var nextMastery = packet.ReadByte() == 1;
            while (nextMastery)
            {
                var mastery = new Mastery(packet.ReadUInt()) {Level = packet.ReadByte()};
                _localPlayer.Masteries.Add(mastery);
                nextMastery = packet.ReadByte() == 1;
            }

            packet.ReadByte();

            var nextSkill = packet.ReadByte() == 1;

            while (nextSkill)
            {
                var skill = new CharacterSkill(packet.ReadUInt()) {Enabled = packet.ReadByte() == 1};
                _localPlayer.Skills.Add(skill);
                nextSkill = packet.ReadByte() == 1;
            }

            var completedQuestCount = packet.ReadUShort();
            completedQuestCount.Repeat(i =>
            {
                var completedQuestId = packet.ReadUInt();
            });

            var activeQuestCount = packet.ReadByte();
            activeQuestCount.Repeat(i =>
            {
                var refQuestId = packet.ReadUInt();
                var achievementCount = packet.ReadByte();
                var autoShareRequired = packet.ReadByte();
                var unk01 = packet.ReadByte();
                var unk02 = packet.ReadByte();
                var questType = (QuestType) packet.ReadByte();

                if (questType == QuestType.TimeLimited)
                {
                    var remainingTime = packet.ReadUInt();
                }

                var questStatus = packet.ReadByte();
                if (questType != QuestType.Objectives)
                {
                    var objectiveCount = packet.ReadByte();
                    objectiveCount.Repeat(j =>
                    {
                        var objectiveId = packet.ReadByte();
                        var objectiveStatus = packet.ReadByte();
                        var objectiveName = packet.ReadAscii();
                        var taskCount = packet.ReadByte();
                        taskCount.Repeat(k =>
                        {
                            var taskValue = packet.ReadUInt();
                        });
                    });
                }

                if (questType != QuestType.Npcs)
                {
                    return;
                }

                var questNpcCount = packet.ReadByte();
                questNpcCount.Repeat(j =>
                {
                    var questNpcId = packet.ReadUInt();
                });
            });

            packet.ReadByte(); // structure changes

            var collectionBookStartedCount = packet.ReadUInt();
            collectionBookStartedCount.Repeat(i =>
            {
                var bookIndex = packet.ReadUInt();
                var bookStartDateTime = packet.ReadUInt();
                var bookPageCount = packet.ReadUInt();
            });

            _localPlayer.Uid = packet.ReadUInt();
            var localPoint = new LocalPoint(
                packet.ReadUShort(),
                packet.ReadFloat(),
                packet.ReadFloat(),
                packet.ReadFloat()
            );

            _localPlayer.Angle = packet.ReadUShort();

            var destinationSet = packet.ReadByte() == 1;
            _localPlayer.WalkMode = (Actor.Movement.Mode) packet.ReadByte();

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
                _localPlayer.Angle = packet.ReadUShort();
            }

            var lifeState = packet.ReadByte();
            packet.ReadByte();
            _localPlayer.Motion = (Actor.Movement.Motion) packet.ReadByte();
            var status = packet.ReadByte();

            packet.ReadByte(); // idk what position, but there is an unknown byte before walkspeed

            _localPlayer.WalkSpeed = packet.ReadFloat();
            _localPlayer.RunSpeed = packet.ReadFloat();
            _localPlayer.ZerkSpeed = packet.ReadFloat();
            var buffCount = packet.ReadByte();
            buffCount.Repeat(i =>
            {
                var refSkillId = packet.ReadUInt();
                var buff = new Buff(refSkillId) {RemainingDuration = packet.ReadUInt()};

                if (buff.IsRecoveryDivision())
                {
                    var isBuffOwner = packet.ReadByte();
                    if (isBuffOwner == 1)
                    {
                        buff.CasterUid = _localPlayer.Uid;
                    }
                }

                if (buff.IsBardAreaBuff())
                {
                    buff.Uid = packet.ReadUInt();
                    var isBuffOwner = packet.ReadByte();
                }

                buff.TargetUid = _localPlayer.Uid;
                _localPlayer.Buffs.Add(buff);
            });

            _localPlayer.Name = packet.ReadAscii();
            _localPlayer.JobName = packet.ReadAscii();
            packet.ReadByte();
            packet.ReadByte();
            var jobType = packet.ReadByte();
            _localPlayer.JobLevel = packet.ReadByte();
            _localPlayer.JobExpGained = packet.ReadUInt();
            var jobContribution = packet.ReadUInt();
            var jobReward = packet.ReadUInt();
            var pvpState = packet.ReadByte();
            var transportFlag = packet.ReadByte();
            var inCombat = packet.ReadByte();

            if (transportFlag == 1)
            {
                var transportUid = packet.ReadUInt();
            }

            var pvpFlag = packet.ReadByte();
            var guideFlag = packet.ReadULong();
            packet.ReadByte();
            packet.ReadByte();
            packet.ReadByte();
            var jid = packet.ReadUInt();

            _localPlayer.LocalPoint = localPoint;

            Program.Gui.RefreshMap(true);
            Entities.Spawned(_localPlayer);
            Program.Gui.InitBindings();

            server.Log("Character spawned");
        }

        #endregion

        #region CelestialPosition

        [PacketHandler(Opcodes.Agent.Response.CHAR_CELESTIAL_POSITION)]
        public void GameReady(Server server, Packet packet)
        {
            server.Inject(new Packet(Opcodes.Agent.Request.GAME_READY));
        }

        #endregion

        #region StatChanged

        [PacketHandler(Opcodes.Agent.Response.CHAR_STAT)]
        public void CharacterStats(Server server, Packet packet)
        {
            var phyMin = packet.ReadUInt();
            var phyMax = packet.ReadUInt();
            var magMin = packet.ReadUInt();
            var magMax = packet.ReadUInt();
            var phyDef = packet.ReadUShort();
            var magDef = packet.ReadUShort();
            var hitRate = packet.ReadUShort();
            var parry = packet.ReadUShort();
            var maxHp = packet.ReadUInt();

            if (_localPlayer.Hp > maxHp)
            {
                _localPlayer.Hp = maxHp;
            }

            _localPlayer.MaxHp = maxHp;

            var maxMp = packet.ReadUInt();
            if (_localPlayer.Mp > maxMp)
            {
                _localPlayer.Mp = maxMp;
            }

            _localPlayer.MaxMp = maxMp;

            var str = packet.ReadUShort();
            var intellect = packet.ReadUShort();
        }

        #endregion

        #region CharInfo

        [PacketHandler(Opcodes.Agent.Response.CHAR_INFO_UPDATE)]
        public void CharacterInfo(Server server, Packet packet)
        {
            var updateType = packet.ReadByte();
            switch (updateType)
            {
                // Gold changed
                case 1:
                    _localPlayer.Gold = packet.ReadULong();
                    break;

                // Sp changed
                case 2:
                    _localPlayer.Skillpoints = packet.ReadUInt();
                    break;
                // Zerk points
                case 4:
                    var newPoints = packet.ReadByte();
                    break;

                default:
                    server.Log("Unhandled characterinfo changetype: " + updateType);
                    server.DebugPacket(packet);
                    break;
            }
        }

        #endregion

        #region ExpGained

        [PacketHandler(Opcodes.Agent.Response.CHAR_XP_UPDATE)]
        public void ExpGained(Server server, Packet packet)
        {
            var sourceUid = packet.ReadUInt();
            var expAmount = packet.ReadLong();
            var spAmount = packet.ReadLong();

            server.Log(expAmount + " EXP");
            server.Log(spAmount + " SP");
        }

        #endregion

        #region Inventory

        [PacketHandler(Opcodes.Agent.Response.INVENTORY_ITEM_USE)]
        public void ItemUse(Server server, Packet packet)
        {
            if (!packet.ReadBool())
            {
                return;
            }

            var slot = packet.ReadByte();
            var quantity = packet.ReadUShort();

            var inventory = _localPlayer.Inventory;
            var changedItem = inventory.FirstOrDefault(x => x.Slot == slot);
            if (changedItem == null)
            {
                return;
            }

            if (quantity == 0)
            {
                inventory.Remove(changedItem);
            }
            else
            {
                changedItem.Quantity = quantity;
            }
        }

        #endregion

        [PacketHandler(Opcodes.Agent.Response.GAME_INVITE)]
        public void ExchangeStart(Server server, Packet packet)
        {
            var type = packet.ReadByte();
            switch (type)
            {
                case 1:
                    Console.WriteLine("exchange invite");
                    break;

                case 2:
                    Console.WriteLine("party invite");
                    break;
            }

            var playerUid = packet.ReadUInt();
            var player = Entities.GetPlayers().FirstOrDefault(x => x.Uid == playerUid);
        }

        #region Utility methods

        public static InventoryItem ParseItem(Packet packet, Locale locale, bool inventory = true)
        {
            var rentType = packet.ReadUInt();

            switch (rentType)
            {
                case 1:
                    var canDelete = packet.ReadUShort();
                    var beginPeriod = packet.ReadULong();
                    var endPeriod = packet.ReadULong();
                    break;

                case 2:
                    var canDelete2 = packet.ReadUShort();
                    var canRecharge = packet.ReadUShort();
                    var meterRateTime = packet.ReadUInt();
                    break;

                case 3:
                    var canDelete3 = packet.ReadUShort();
                    var canRecharge2 = packet.ReadUShort();
                    var beginPeriod2 = packet.ReadUInt();
                    var endPeriod2 = packet.ReadUInt();
                    var packingTime = packet.ReadUInt();
                    break;
            }

            var refItemId = packet.ReadUInt();
            var inventoryItem = InventoryItem.FromId(refItemId);

            switch (inventoryItem.TypeId2)
            {
                case 1:
                case 4: // job gear
                case 5:
                    var plus = packet.ReadByte();
                    var variance = packet.ReadULong();
                    var dura = packet.ReadUInt();

                    var magicOptions = packet.ReadByte();
                    magicOptions.Repeat(j =>
                    {
                        var paramType = packet.ReadUInt();
                        var paramValue = packet.ReadUInt();
                    });

                    // 1 = sockets
                    packet.ReadByte();
                    var sockets = packet.ReadByte();
                    sockets.Repeat(j =>
                    {
                        var socketSlot = packet.ReadByte();
                        var socketParam = packet.ReadUInt();
                        var socketId = packet.ReadUInt();
                    });

                    // 2 = adv elixirs
                    packet.ReadByte();
                    var advElixirs = packet.ReadByte();
                    advElixirs.Repeat(j =>
                    {
                        var advElixirSlot = packet.ReadByte();
                        var advElixirId = packet.ReadUInt();
                        var advElixirValue = packet.ReadUInt();
                    });

                    if (locale.IsInternational())
                    {
                        // 3 = ??
                        packet.ReadByte();
                        var unk01 = packet.ReadByte();
                        unk01.Repeat(j =>
                        {
                            var unkSlot = packet.ReadByte();
                            var unkParam1 = packet.ReadUInt();
                            var unkParam2 = packet.ReadUInt();
                        });

                        // 4 = ??
                        packet.ReadByte();
                        var unk02 = packet.ReadByte();
                        unk02.Repeat(j =>
                        {
                            var unkSlot = packet.ReadByte();
                            var unkParam1 = packet.ReadUInt();
                            var unkParam2 = packet.ReadUInt();
                        });
                    }

                    break;

                case 2:
                    switch (inventoryItem.TypeId3)
                    {
                        case 1:
                            var state = packet.ReadByte();
                            var refObjId = packet.ReadUInt();
                            var name = packet.ReadAscii();

                            switch (inventoryItem.TypeId4)
                            {
                                case 1:
                                case 3:
                                    var level = packet.ReadByte();
                                    break;

                                case 2:
                                    var rentTimeEndSeconds = packet.ReadUInt();
                                    break;
                            }

                            if (locale.IsInternational() && inventory)
                            {
                                packet.ReadByte();
                            }

                            break;

                        case 2:
                            var refObjId2 = packet.ReadUInt();
                            break;

                        case 3:
                            var quantity = packet.ReadUInt();
                            break;
                    }

                    break;

                case 3:
                    var stackCount = packet.ReadUShort();

                    inventoryItem.Quantity = stackCount;

                    if (inventoryItem.TypeId3 == 11 && inventoryItem.TypeId4 is 1 or 2)
                    {
                        var assimilationProb = packet.ReadByte();
                        break;
                    }

                    if (inventoryItem.TypeId3 == 14 && inventoryItem.TypeId4 == 2)
                    {
                        var magParams = packet.ReadByte();
                        magParams.Repeat(j =>
                        {
                            var paramType = packet.ReadUInt();
                            var paramValue = packet.ReadUInt();
                        });
                    }

                    break;
            }

            return inventoryItem;
        }

        public static List<InventoryItem> ParseInventory(Packet packet, byte itemCount, Locale locale,
            bool inventory = true)
        {
            var items = new List<InventoryItem>();

            itemCount.Repeat(i =>
            {
                var slot = packet.ReadByte();
                var inventoryItem = ParseItem(packet, locale, inventory);
                inventoryItem.Slot = slot;

                items.Add(inventoryItem);
            });

            return items;
        }

        #endregion
    }
}