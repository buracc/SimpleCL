using System;
using System.Collections.Generic;
using System.Linq;
using SilkroadSecurityApi;
using SimpleCL.Database;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Quests;
using SimpleCL.Enums.Server;
using SimpleCL.Enums.Skills;
using SimpleCL.Interaction.Entities;
using SimpleCL.Models.Character;
using SimpleCL.Models.Coordinates;
using SimpleCL.Models.Items;
using SimpleCL.Network;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Game.Entities
{
    public class LocalPlayerService : Service
    {
        private readonly SilkroadServer _silkroadServer;
        private readonly Gateway _gateway;

        public LocalPlayerService(SilkroadServer silkroadServer, Gateway gateway)
        {
            _silkroadServer = silkroadServer;
            _gateway = gateway;
        }

        [PacketHandler(Opcodes.Agent.Response.CHAR_DATA_CHUNK)]
        public void GameJoined(Server server, Packet packet)
        {
            _gateway.Dispose();
            LocalPlayer local = LocalPlayer.Get;

            var serverTime = packet.ReadUInt();
            var refObjId = packet.ReadUInt();
            // LocalPlayer.Refresh(refObjId);
            var scale = packet.ReadByte();
            local.Level = packet.ReadByte();
            var maxLevel = packet.ReadByte();
            local.ExpGained = packet.ReadULong();
            var spOffset = packet.ReadUInt();
            local.Gold = packet.ReadULong();
            local.Skillpoints = packet.ReadUInt();
            var statPoint = packet.ReadUShort();
            var zerkPoints = packet.ReadByte();
            var gatheredExp = packet.ReadUInt(); // GatheredExp according to DaxterSoul, but it's wrong on TRSRO
            local.MaxHp = packet.ReadUInt();
            local.MaxMp = packet.ReadUInt();
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

            List<InventoryItem> inv = ParseInventory(packet, itemCount);

            local.Inventories["inventory"] = inv.Where(x => x.Slot > 12).ToList();
            local.Inventories["equipment"] = inv.Where(x => x.Slot < 13).ToList();

            var avatarInventorySize = packet.ReadByte();
            var avatarInventoryCount = packet.ReadByte();

            local.Inventories["avatar"] = ParseInventory(packet, avatarInventoryCount, false);

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

                local.Inventories["jobEquipment"] = ParseInventory(packet, jobInventoryCount, false);
            }

            packet.ReadByte();

            var nextMastery = packet.ReadByte() == 1;
            while (nextMastery)
            {
                var masteryId = packet.ReadUInt();
                var masteryLevel = packet.ReadByte();
                nextMastery = packet.ReadByte() == 1;
            }

            packet.ReadByte();

            var nextSkill = packet.ReadByte() == 1;

            while (nextSkill)
            {
                var skillId = packet.ReadUInt();
                var skillEnabled = packet.ReadByte() == 1;

                if (skillEnabled)
                {
                    var skillData = GameDatabase.Get.GetSkill(skillId);
                }

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

                if (questType == QuestType.Npcs)
                {
                    var questNpcCount = packet.ReadByte();
                    questNpcCount.Repeat(j =>
                    {
                        var questNpcId = packet.ReadUInt();
                    });
                }
            });

            packet.ReadByte(); // structure changes

            var collectionBookStartedCount = packet.ReadUInt();
            collectionBookStartedCount.Repeat(i =>
            {
                var bookIndex = packet.ReadUInt();
                var bookStartDateTime = packet.ReadUInt();
                var bookPageCount = packet.ReadUInt();
            });

            local.Uid = packet.ReadUInt();
            var localPoint = new LocalPoint(
                packet.ReadUShort(),
                packet.ReadFloat(),
                packet.ReadFloat(),
                packet.ReadFloat()
            );

            local.Angle = packet.ReadUShort();

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
                local.Angle = packet.ReadUShort();
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

            local.Name = packet.ReadAscii();
            local.JobName = packet.ReadAscii();
            packet.ReadByte();
            packet.ReadByte();
            var jobType = packet.ReadByte();
            local.JobLevel = packet.ReadByte();
            local.JobExpGained = packet.ReadUInt();
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

            local.LocalPoint = localPoint;

            SimpleCL.Gui.StartRefreshTimer();
            SimpleCL.Gui.AddMinimapMarker(LocalPlayer.Get);

            server.Log("Successfully joined the game");
        }

        [PacketHandler(Opcodes.Agent.Response.CHAR_CELESTIAL_POSITION)]
        public void GameReady(Server server, Packet packet)
        {
            server.Inject(new Packet(Opcodes.Agent.Request.GAME_READY));
        }

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
            LocalPlayer.Get.MaxHp = packet.ReadUInt();
            LocalPlayer.Get.MaxMp = packet.ReadUInt();
            var str = packet.ReadUShort();
            var intellect = packet.ReadUShort();
        }

        [PacketHandler(Opcodes.Agent.Response.CHAR_INFO_UPDATE)]
        public void CharacterInfo(Server server, Packet packet)
        {
            var updateType = packet.ReadByte();
            switch (updateType)
            {
                // Gold changed
                case 1:
                    LocalPlayer.Get.Gold = packet.ReadULong();
                    break;

                // Sp changed
                case 2:
                    LocalPlayer.Get.Skillpoints = packet.ReadUInt();
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

        private List<InventoryItem> ParseInventory(Packet packet, byte itemCount, bool inventory = true)
        {
            List<InventoryItem> items = new List<InventoryItem>();

            itemCount.Repeat(i =>
            {
                var slot = packet.ReadByte();
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
                var itemData = GameDatabase.Get.GetItemData(refItemId);
                var typeId2 = byte.Parse(itemData["tid1"]);
                var typeId3 = byte.Parse(itemData["tid2"]);
                var typeId4 = byte.Parse(itemData["tid3"]);

                InventoryItem inventoryItem =
                    new InventoryItem(slot, refItemId, itemData["servername"], itemData["name"]);

                switch (typeId2)
                {
                    case 1:
                    case 4: // job gear
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
                            var socketId = packet.ReadUInt();
                            var socketParam = packet.ReadByte();
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

                        if (_silkroadServer.Locale.IsInternational())
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
                        switch (typeId3)
                        {
                            case 1:
                                var state = packet.ReadByte();
                                var refObjId = packet.ReadUInt();
                                var name = packet.ReadAscii();

                                if (typeId4 == 2)
                                {
                                    var rentTimeEndSeconds = packet.ReadUInt();
                                }

                                if (_silkroadServer.Locale.IsInternational() && inventory)
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

                        if (typeId3 == 11 && (typeId4 == 1 || typeId4 == 2))
                        {
                            var assimilationProb = packet.ReadByte();
                            break;
                        }

                        if (typeId3 == 14 && typeId4 == 2)
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

                items.Add(inventoryItem);
            });

            return items;
        }
    }
}