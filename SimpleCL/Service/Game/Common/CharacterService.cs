using SilkroadSecurityApi;
using SimpleCL.Database;
using SimpleCL.Enums.Common;
using SimpleCL.Enums.Server;
using SimpleCL.Model.Character;
using SimpleCL.Model.Coord;
using SimpleCL.Network;

namespace SimpleCL.Service.Game.Common
{
    public class CharacterService : Service
    {
        private readonly SilkroadServer _silkroadServer;

        public CharacterService(SilkroadServer silkroadServer)
        {
            _silkroadServer = silkroadServer;
        }
        
        [PacketHandler(Opcodes.Agent.Response.CHAR_DATA_CHUNK)]
        public void GameJoined(Server server, Packet packet)
        {
            var serverTime = packet.ReadUInt32();
            var objId = packet.ReadUInt32();
            var scale = packet.ReadUInt8();
            var level = packet.ReadUInt8();
            var maxLevel = packet.ReadUInt8();
            var expGained = packet.ReadUInt64();
            var spOffset = packet.ReadUInt32();
            var gold = packet.ReadUInt64();
            var sp = packet.ReadUInt32();
            var statPoint = packet.ReadUInt16();
            var zerkPoints = packet.ReadUInt8();
            var gatheredExp = packet.ReadUInt32(); // GatheredExp according to DaxterSoul, but it's wrong on TRSRO
            var maxHp = packet.ReadUInt32();
            var maxMp = packet.ReadUInt32();
            var icon = packet.ReadUInt8();
            var dailyPk = packet.ReadUInt8();
            var totalPk = packet.ReadUInt16();
            var pkPoint = packet.ReadUInt32();
            var zerkLevel = packet.ReadUInt8();
            var freePvp = packet.ReadUInt8();

            if (_silkroadServer.Locale.IsInternational())
            {
                for (int i = 0; i < 25; i++)
                {
                    packet.ReadUInt8();
                }
            }

            var inventorySize = packet.ReadUInt8();
            var itemCount = packet.ReadUInt8();

            ParseInventory(packet, itemCount);

            var avatarInventorySize = packet.ReadUInt8();
            var avatarInventoryCount = packet.ReadUInt8();

            ParseInventory(packet, avatarInventoryCount, false);

            if (_silkroadServer.Locale.IsInternational())
            {
                var jobPouchSize = packet.ReadUInt8();
                var jobPouchCount = packet.ReadUInt8();

                // parse job pouch inventory

                var jobInventorySize = packet.ReadUInt8();
                var jobInventoryCount = packet.ReadUInt8();

                ParseInventory(packet, jobInventoryCount, false);
            }

            if (_silkroadServer.Locale.IsInternational())
            {
                packet.ReadUInt8();
            }

            var nextMastery = packet.ReadUInt8() == 1;

            while (nextMastery)
            {
                var masteryId = packet.ReadUInt32();
                var masteryLevel = packet.ReadUInt8();
                nextMastery = packet.ReadUInt8() == 1;
            }

            packet.ReadUInt8();

            var nextSkill = packet.ReadUInt8() == 1;

            while (nextSkill)
            {
                var skillId = packet.ReadUInt32();
                var skillEnabled = packet.ReadUInt8() == 1;

                if (skillEnabled)
                {
                    var skillData = GameDatabase.GetInstance().GetSkill(skillId);
                }

                nextSkill = packet.ReadUInt8() == 1;
            }

            Character local = new Character(
                maxHp, maxMp, level, expGained, sp, gold, new Coordinates(0, 0, 0, 0, 0)
            );


            Program.Gui.Character = local;
            Program.Gui.RefreshGui();

            server.Log("Successfully joined the game");
        }

        [PacketHandler(Opcodes.Agent.Response.CHAR_CELESTIAL_POSITION)]
        public void GameReady(Server server, Packet packet)
        {
            server.Inject(new Packet(Opcodes.Agent.Request.GAME_READY));
        }

        public void ParseInventory(Packet packet, byte itemCount, bool inventory = true)
        {
            for (int i = 0; i < itemCount; i++)
            {
                var slot = packet.ReadUInt8();
                var rentType = packet.ReadUInt32();

                switch (rentType)
                {
                    case 1:
                        var canDelete = packet.ReadUInt16();
                        var beginPeriod = packet.ReadUInt64();
                        var endPeriod = packet.ReadUInt64();
                        break;

                    case 2:
                        var canDelete2 = packet.ReadUInt16();
                        var canRecharge = packet.ReadUInt16();
                        var meterRateTime = packet.ReadUInt32();
                        break;

                    case 3:
                        var canDelete3 = packet.ReadUInt16();
                        var canRecharge2 = packet.ReadUInt16();
                        var beginPeriod2 = packet.ReadUInt32();
                        var endPeriod2 = packet.ReadUInt32();
                        var packingTime = packet.ReadUInt32();
                        break;
                }

                var refItemId = packet.ReadUInt32();
                var itemData = GameDatabase.GetInstance().GetItemData(refItemId);
                var typeId2 = byte.Parse(itemData["tid1"]);
                var typeId3 = byte.Parse(itemData["tid2"]);
                var typeId4 = byte.Parse(itemData["tid3"]);

                switch (typeId2)
                {
                    case 1:
                    case 4: // job gear
                        var plus = packet.ReadUInt8();
                        var variance = packet.ReadUInt64();
                        var dura = packet.ReadUInt32();

                        var magicOptions = packet.ReadUInt8();
                        for (int j = 0; j < magicOptions; j++)
                        {
                            var paramType = packet.ReadUInt32();
                            var paramValue = packet.ReadUInt32();
                        }

                        // 1 = sockets
                        packet.ReadUInt8();
                        var sockets = packet.ReadUInt8();
                        for (int j = 0; j < sockets; j++)
                        {
                            var socketSlot = packet.ReadUInt8();
                            var socketId = packet.ReadUInt32();
                            var socketParam = packet.ReadUInt8();
                        }

                        // 2 = adv elixirs
                        packet.ReadUInt8();
                        var advElixirs = packet.ReadUInt8();
                        for (int j = 0; j < advElixirs; j++)
                        {
                            var advElixirSlot = packet.ReadUInt8();
                            var advElixirId = packet.ReadUInt32();
                            var advElixirValue = packet.ReadUInt32();
                        }

                        if (_silkroadServer.Locale.IsInternational())
                        {
                            // 3 = ??
                            packet.ReadUInt8();
                            var unk01 = packet.ReadUInt8();
                            for (int j = 0; j < unk01; j++)
                            {
                                var unkSlot = packet.ReadUInt8();
                                var unkParam1 = packet.ReadUInt32();
                                var unkParam2 = packet.ReadUInt32();
                            }

                            // 4 = ??
                            packet.ReadUInt8();
                            var unk02 = packet.ReadUInt8();
                            for (int j = 0; j < unk01; j++)
                            {
                                var unkSlot = packet.ReadUInt8();
                                var unkParam1 = packet.ReadUInt32();
                                var unkParam2 = packet.ReadUInt32();
                            }
                        }

                        break;

                    case 2:
                        switch (typeId3)
                        {
                            case 1:
                                var state = packet.ReadUInt8();
                                var refObjId = packet.ReadUInt32();
                                var name = packet.ReadAscii();

                                if (typeId4 == 2)
                                {
                                    var rentTimeEndSeconds = packet.ReadUInt32();
                                }

                                if (_silkroadServer.Locale.IsInternational() && inventory)
                                {
                                    packet.ReadUInt8();
                                }

                                break;

                            case 2:
                                var refObjId2 = packet.ReadUInt32();
                                break;

                            case 3:
                                var quantity = packet.ReadUInt32();
                                break;
                        }

                        break;

                    case 3:
                        var stackCount = packet.ReadUInt16();
                        if (typeId3 == 11 && (typeId4 == 1 || typeId4 == 2))
                        {
                            var assimilationProb = packet.ReadUInt8();
                            break;
                        }

                        if (typeId3 == 14 && typeId4 == 2)
                        {
                            var magParams = packet.ReadUInt8();
                            for (int magParamIndex = 0; magParamIndex < magParams; magParamIndex++)
                            {
                                var paramType = packet.ReadUInt32();
                                var paramValue = packet.ReadUInt32();
                            }
                        }

                        break;
                }
            }
        }
    }
}