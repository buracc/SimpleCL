using System;
using System.Collections.Generic;
using System.Text;
using SilkroadSecurityApi;
using SimpleCL.Model.Game;
using SimpleCL.Model.Server;
using SimpleCL.Network;
using SimpleCL.Network.Enums;
using SimpleCL.Util;

namespace SimpleCL.Service.Login
{
    public class LoginService : Service
    {
        [PacketHandler(Opcodes.IDENTITY)]
        public void SendIdentity(Server server, Packet packet)
        {
            if (server is Gateway)
            {
                Packet identity = new Packet(Opcodes.Gateway.Request.PATCH, true);
                identity.WriteUInt8(Locale.SRO_TR_Official_GameGami);
                identity.WriteAscii("SR_Client");
                identity.WriteUInt32(Gateway.TrsroVersion);
                server.Inject(identity);
                return;
            }

            if (server is Agent)
            {
                Packet login = new Packet(Opcodes.Agent.Request.AUTH, true);
                login.WriteUInt32(((Agent) server).SessionId);
                login.WriteAscii(Credentials.Username);
                login.WriteAscii(Credentials.Password);
                login.WriteUInt8(Locale.SRO_TR_Official_GameGami);
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
            Dictionary<string, SilkroadServer> servers = new Dictionary<string, SilkroadServer>();

            while (packet.ReadUInt8() == 1)
            {
                byte farmId = packet.ReadUInt8();
                string farmName = packet.ReadAscii();
            }

            server.Log("Servers:");
            while (packet.ReadUInt8() == 1)
            {
                SilkroadServer silkroadServer = new SilkroadServer(
                    packet.ReadUInt16(),
                    packet.ReadAscii(),
                    (ServerCapacity) packet.ReadUInt8(),
                    packet.ReadUInt8() == 1
                );

                servers[silkroadServer.Name] = silkroadServer;

                server.Log(silkroadServer.ToString());
            }

            Packet login = new Packet(Opcodes.Gateway.Request.LOGIN2, true);
            login.WriteUInt8(Locale.SRO_TR_Official_GameGami);
            login.WriteAscii(Credentials.Username);
            login.WriteAscii(Credentials.Password);
            login.WriteUInt8Array(NetworkUtils.GetMacAddressBytes());
            login.WriteUInt16(servers[Credentials.Server].Id);
            login.WriteUInt8(1);

            server.Inject(login);
        }

        [PacketHandler(Opcodes.Gateway.Response.LOGIN2)]
        public void SendPasscode(Server server, Packet packet)
        {
            byte[] key = {0x0F, 0x07, 0x3D, 0x20, 0x56, 0x62, 0xC9, 0xEB};
            Blowfish blowfish = new Blowfish();
            blowfish.Initialize(key);
            byte[] encodedPasscode = Encoding.ASCII.GetBytes(Credentials.Passcode);
            byte[] encryptedPasscode = blowfish.Encode(encodedPasscode);

            Packet passcode = new Packet(Opcodes.Gateway.Request.PASSCODE, true);
            passcode.WriteUInt8(4);
            passcode.WriteUInt16(Credentials.Passcode.Length);
            passcode.WriteUInt8Array(encryptedPasscode);

            server.Inject(passcode);
        }

        [PacketHandler(Opcodes.Gateway.Response.PASSCODE)]
        public void PasscodeResponse(Server server, Packet packet)
        {
            packet.ReadUInt8();
            byte passcodeResult = packet.ReadUInt8();
            if (passcodeResult == 2)
            {
                byte attempts = packet.ReadUInt8();
                server.Log("Invalid passcode. Attempts: [" + attempts + "/" + 3 + "]");
                server.Close();
            }
        }

        [PacketHandler(Opcodes.Gateway.Response.AGENT_AUTH)]
        public void AgentAuth(Server server, Packet packet)
        {
            byte result = packet.ReadUInt8();
            switch (result)
            {
                case 1:
                    uint sessionId = packet.ReadUInt32();
                    string agentIp = packet.ReadAscii();
                    ushort agentPort = packet.ReadUInt16();

                    Agent agent = new Agent(agentIp, agentPort, (byte) Locale.SRO_TR_Official_GameGami, sessionId);
                    agent.RegisterService(this);
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

                                    server.Log("Account banned: " + reason + "\nEnd date: " +
                                               endDate);
                                    break;

                                case LoginBlockType.AccountInspection:
                                    server.Log("Unable to connect due to inspection.");
                                    break;
                            }

                            break;

                        case LoginErrorCode.AlreadyConnected:
                            server.Log("Account is already connected.");
                            break;

                        case LoginErrorCode.IPLimit:
                            server.Log("IP limit exceeded.");
                            break;

                        case LoginErrorCode.ServerIsFull:
                            server.Log("Server is full.");
                            break;
                    }

                    server.Close();
                    break;

                case 3:
                    server.Log("Unhandled login result.");
                    server.Close();
                    return;
            }
        }
        
        [PacketHandler(Opcodes.Agent.Response.AUTH)]
        public void EnterCharacterSelect(Server server, Packet packet)
        {
            bool result = packet.ReadUInt8() == 1;
            if (result)
            {
                AuthErrorCode error = (AuthErrorCode) packet.ReadUInt8();
                switch (error)
                {
                    case AuthErrorCode.IpLimit:
                        server.Log("IP limit exceeded.");
                        return;

                    case AuthErrorCode.ServerFull:
                        server.Log("Server is full.");
                        return;
                }

                Packet charSelect = new Packet(Opcodes.Agent.Request.CHARACTER_SELECTION_ACTION);
                charSelect.WriteUInt8(2);
                server.Inject(charSelect);
            }
        }

        [PacketHandler(Opcodes.Agent.Response.CHARACTER_SELECTION_ACTION)]
        public void SelectCharacter(Server server, Packet packet)
        {
            Agent agent = (Agent) server;
            byte action = packet.ReadUInt8();
            bool succeeded = packet.ReadUInt8() == 1;

            if (action == 2 && succeeded)
            {
                byte charCount = packet.ReadUInt8();

                Dictionary<string, CharSelect> characters = new Dictionary<string, CharSelect>();

                for (int i = 0; i < charCount; i++)
                {
                    packet.ReadUInt32();
                    string name = packet.ReadAscii();

                    if (agent.Locale == (byte) Locale.SRO_TR_Official_GameGami)
                    {
                        packet.ReadUInt16();
                    }

                    packet.ReadUInt8();
                    byte level = packet.ReadUInt8();
                    packet.ReadUInt64();
                    packet.ReadUInt16();
                    packet.ReadUInt16();
                    packet.ReadUInt16();

                    if (agent.Locale == (byte) Locale.SRO_TR_Official_GameGami)
                    {
                        packet.ReadUInt32();
                    }

                    packet.ReadUInt32();
                    packet.ReadUInt32();

                    if (agent.Locale == (byte) Locale.SRO_TR_Official_GameGami)
                    {
                        packet.ReadUInt16();
                    }

                    bool deleting = packet.ReadUInt8() == 1;

                    if (agent.Locale == (byte) Locale.SRO_TR_Official_GameGami)
                    {
                        packet.ReadUInt32();
                    }

                    CharSelect character = new CharSelect(name, level, deleting);

                    if (deleting)
                    {
                        uint minutes = packet.ReadUInt32();
                        character.DeletionTime = DateTime.Now.AddMinutes(minutes);
                        ;
                    }

                    characters[character.Name] = character;
                }

                server.Log("Characters:");
                foreach (var c in characters)
                {
                    server.Log(c.Value.ToString());
                }

                Packet characterJoin = new Packet(Opcodes.Agent.Request.CHARACTER_SELECTION_JOIN);
                characterJoin.WriteAscii(Credentials.CharName);
                server.Inject(characterJoin);
            }
        }

        [PacketHandler(Opcodes.Agent.Response.CHAR_DATA_CHUNK)]
        public void GameJoined(Server server, Packet packet)
        {
            server.Log("Successfully joined the game.");
        }
        
        [PacketHandler(Opcodes.Agent.Response.CHAR_CELESTIAL_POSITION)]
        public void GameReady(Server server, Packet packet)
        {
            server.Inject(new Packet(Opcodes.Agent.Request.GAME_READY));
        }

        [PacketHandler(Opcodes.Agent.Response.CHAT_UPDATE)]
        public void ChatUpdated(Server server, Packet packet)
        {
            ChatChannel channel = (ChatChannel) packet.ReadUInt8();
            ChatMessage chatMessage = new ChatMessage(channel);

            if (channel == ChatChannel.General || channel == ChatChannel.GM ||
                channel == ChatChannel.NPC)
            {
                uint senderID = packet.ReadUInt32();
                chatMessage.SenderID = senderID;
            }
            else
            {
                string senderName = packet.ReadAscii();
                chatMessage.SenderName = senderName;
            }

            string message = packet.ReadUnicode();
            chatMessage.Message = message;
                            
            server.Log(chatMessage.ToString());
        }
    }
}