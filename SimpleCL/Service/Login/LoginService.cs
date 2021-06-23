using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SilkroadSecurityApi;
using SimpleCL.Database;
using SimpleCL.Enums;
using SimpleCL.Enums.Common;
using SimpleCL.Enums.Login;
using SimpleCL.Enums.Server;
using SimpleCL.Model;
using SimpleCL.Model.Character;
using SimpleCL.Model.Coord;
using SimpleCL.Model.Server;
using SimpleCL.Network;
using SimpleCL.Service.Game;
using SimpleCL.Ui;
using SimpleCL.Util;
using CharacterSelect = SimpleCL.Model.Character.CharacterSelect;

namespace SimpleCL.Service.Login
{
    public class LoginService : Service
    {
        private readonly string _username;
        private readonly string _password;
        private readonly SilkroadServer _silkroadServer;

        public LoginService(string username, string password, SilkroadServer silkroadServer)
        {
            _username = username;
            _password = password;
            _silkroadServer = silkroadServer;
        }

        [PacketHandler(Opcodes.IDENTITY)]
        public void SendIdentity(Server server, Packet packet)
        {
            if (server is Gateway)
            {
                Packet identity = new Packet(Opcodes.Gateway.Request.PATCH, true);
                identity.WriteUInt8(_silkroadServer.Locale);
                identity.WriteAscii("SR_Client");
                identity.WriteUInt32(GameDatabase.GetInstance().GetGameVersion());
                server.Inject(identity);
                return;
            }

            if (server is Agent)
            {
                Packet login = new Packet(Opcodes.Agent.Request.AUTH, true);
                login.WriteUInt32(((Agent) server).SessionId);
                login.WriteAscii(_username);
                login.WriteAscii(_password);
                login.WriteUInt8(_silkroadServer.Locale);
                login.WriteUInt8Array(NetworkUtils.GetMacAddressBytes());

                server.Inject(login);
            }
        }

        [PacketHandler(Opcodes.Gateway.Response.PATCH)]
        public void RequestServerlist(Server server, Packet packet)
        {
            server.Inject(new Packet(Opcodes.Gateway.Request.SERVERLIST, true));
        }

        [PacketHandler(Opcodes.Gateway.Response.SERVERLIST)]
        public void SendLogin(Server server, Packet packet)
        {
            List<GameServer> servers = new List<GameServer>();

            while (packet.ReadUInt8() == 1)
            {
                byte farmId = packet.ReadUInt8();
                string farmName = packet.ReadAscii();
            }

            while (packet.ReadUInt8() == 1)
            {
                GameServer gameServer = new GameServer(
                    packet.ReadUInt16(),
                    packet.ReadAscii(),
                    (ServerCapacity) packet.ReadUInt8(),
                    packet.ReadUInt8() == 1
                );

                servers.Add(gameServer);
            }

            Application.Run(new Serverlist(servers, server));
        }

        [PacketHandler(Opcodes.Gateway.Response.LOGIN2)]
        public void SendPasscode(Server server, Packet packet)
        {
            Application.Run(new PasscodeEnter(server));
        }

        [PacketHandler(Opcodes.Gateway.Response.PASSCODE)]
        public void PasscodeResponse(Server server, Packet packet)
        {
            packet.ReadUInt8();
            byte passcodeResult = packet.ReadUInt8();
            if (passcodeResult == 2)
            {
                byte attempts = packet.ReadUInt8();
                Application.Run(new PasscodeEnter(server, "Invalid passcode [" + attempts + "/" + 3 + "]"));
                server.Log("Invalid passcode. Attempts: [" + attempts + "/" + 3 + "]");
            }
        }

        [PacketHandler(Opcodes.Gateway.Response.AGENT_AUTH)]
        public void AgentAuth(Server server, Packet packet)
        {
            byte result = packet.ReadUInt8();
            switch (result)
            {
                case 1:
                    server.Dispose(); // Close gateway connection

                    uint sessionId = packet.ReadUInt32();
                    string agentIp = packet.ReadAscii();
                    ushort agentPort = packet.ReadUInt16();

                    Agent agent = new Agent(agentIp, agentPort, sessionId);
                    agent.RegisterService(this);
                    agent.RegisterService(new ChatService());
                    agent.Debug = true;
                    agent.Start();
                    break;

                case 2:
                    LoginErrorCode errorCode = (LoginErrorCode) packet.ReadUInt8();

                    switch (errorCode)
                    {
                        case LoginErrorCode.InvalidCredentials:
                            uint maxAttempts = packet.ReadUInt32();
                            uint currentAttempts = packet.ReadUInt32();
                            server.Log("Invalid credentials. Attempts: [" + currentAttempts +
                                       "/" + maxAttempts + "]");
                            break;

                        case LoginErrorCode.Blocked:
                            LoginBlockType blockType = (LoginBlockType) packet.ReadUInt8();

                            switch (blockType)
                            {
                                case LoginBlockType.Punishment:
                                    string reason = packet.ReadAscii();
                                    DateTime endDate = new DateTime(
                                        packet.ReadUInt16(),
                                        packet.ReadUInt16(),
                                        packet.ReadUInt16(),
                                        packet.ReadUInt16(),
                                        packet.ReadUInt16(),
                                        packet.ReadUInt16(),
                                        packet.ReadUInt16()
                                    );

                                    server.Log("Account banned: " + reason);
                                    server.Log("End date: " + endDate);
                                    break;

                                case LoginBlockType.AccountInspection:
                                    server.Log("Unable to connect due to inspection.");
                                    break;
                                
                                default:
                                    server.Log("Unhandled block type code: " + blockType);
                                    break;
                            }

                            break;

                        case LoginErrorCode.AlreadyConnected:
                            server.Log("Account is already connected.");
                            break;

                        case LoginErrorCode.Inspection:
                            server.Log("Server is under inspection.");
                            break;

                        case LoginErrorCode.IPLimit:
                            server.Log("IP limit exceeded.");
                            break;

                        case LoginErrorCode.ServerIsFull:
                            server.Log("Server is full.");
                            break;
                        default:
                            server.Log("Unhandled login error code: " + errorCode);
                            break;
                    }

                    server.Disconnect();
                    break;

                case 3:
                    server.Log("Unhandled login result.");
                    server.Disconnect();
                    return;
            }
        }

        [PacketHandler(Opcodes.Agent.Response.AUTH)]
        public void EnterCharacterSelect(Server server, Packet packet)
        {
            byte result = packet.ReadUInt8();
            if (result == 1)
            {
                Packet charSelect = new Packet(Opcodes.Agent.Request.CHARACTER_SELECTION_ACTION);
                charSelect.WriteUInt8(2);
                server.Inject(charSelect);
                return;
            }

            if (result == 2)
            {
                byte errorCode = packet.ReadUInt8();
                AuthErrorCode error = (AuthErrorCode) errorCode;
                switch (error)
                {
                    case AuthErrorCode.InvalidCredentials:
                        server.Log("Invalid credentials.");
                        break;
                        
                    case AuthErrorCode.IpLimit:
                        server.Log("IP limit exceeded.");
                        break;

                    case AuthErrorCode.ServerFull:
                        server.Log("Server is full.");
                        break;
                    
                    default:
                        server.Log("Unhandled auth error code: " + error);
                        break;
                }
                
                server.Disconnect();
            }
        }

        [PacketHandler(Opcodes.Agent.Response.CHARACTER_SELECTION_ACTION)]
        public void SelectCharacter(Server server, Packet packet)
        {
            byte action = packet.ReadUInt8();
            bool succeeded = packet.ReadUInt8() == 1;

            if (action == 2 && succeeded)
            {
                byte charCount = packet.ReadUInt8();

                List<CharacterSelect> chars = new List<CharacterSelect>();

                for (int i = 0; i < charCount; i++)
                {
                    packet.ReadUInt32();
                    string name = packet.ReadAscii();
                    if (_silkroadServer.Locale.IsInternational())
                    {
                        string jobName = packet.ReadAscii();
                    }

                    packet.ReadUInt8();
                    byte level = packet.ReadUInt8();
                    packet.ReadUInt64();
                    packet.ReadUInt16();
                    packet.ReadUInt16();
                    packet.ReadUInt16();

                    if (_silkroadServer.Locale.IsInternational())
                    {
                        packet.ReadUInt32();
                    }

                    uint hp = packet.ReadUInt32();
                    uint mp = packet.ReadUInt32();

                    if (_silkroadServer.Locale.IsInternational())
                    {
                        packet.ReadUInt16();
                    }

                    bool deleting = packet.ReadUInt8() == 1;

                    if (_silkroadServer.Locale.IsInternational())
                    {
                        packet.ReadUInt32();
                    }

                    CharacterSelect character = new CharacterSelect(name, level, deleting);

                    if (deleting)
                    {
                        uint minutes = packet.ReadUInt32();
                        character.DeletionTime = DateTime.Now.AddMinutes(minutes);
                    }

                    byte guildMemberClass = packet.ReadUInt8();

                    bool guildRenameRequired = packet.ReadUInt8() == 1;
                    if (guildRenameRequired)
                    {
                        string guildName = packet.ReadAscii();
                    }

                    byte academyMemberClass = packet.ReadUInt8();
                    byte itemCount = packet.ReadUInt8();

                    Console.WriteLine(itemCount);

                    for (int j = 0; j < itemCount; j++)
                    {
                        uint refItemId = packet.ReadUInt32();
                        byte plus = packet.ReadUInt8();
                    }

                    byte avatarItemCount = packet.ReadUInt8();
                    for (int j = 0; j < avatarItemCount; j++)
                    {
                        uint refItemId = packet.ReadUInt32();
                        byte plus = packet.ReadUInt8();
                    }

                    chars.Add(character);
                }

                Application.Run(new Ui.CharacterSelect(chars, server));
            }
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