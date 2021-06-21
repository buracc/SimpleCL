using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using SilkroadSecurityApi;
using SimpleCL.Model.Game;
using SimpleCL.Model.Server;
using SimpleCL.Network;
using SimpleCL.Network.Enums;
using SimpleCL.Ui;
using SimpleCL.Util;

namespace SimpleCL.Service.Login
{
    public class LoginService : Service
    {
        private readonly string _username;
        private readonly string _password;
        private readonly Locale _locale;

        public LoginService(string username, string password, Locale locale)
        {
            _username = username;
            _password = password;
            _locale = locale;
        }

        [PacketHandler(Opcodes.IDENTITY)]
        public void SendIdentity(Server server, Packet packet)
        {
            if (server is Gateway)
            {
                Packet identity = new Packet(Opcodes.Gateway.Request.PATCH, true);
                identity.WriteUInt8(_locale);
                identity.WriteAscii("SR_Client");
                identity.WriteUInt32(Gateway.TrsroVersion);
                server.Inject(identity);
                return;
            }

            if (server is Agent)
            {
                Packet login = new Packet(Opcodes.Agent.Request.AUTH, true);
                login.WriteUInt32(((Agent) server).SessionId);
                login.WriteAscii(_username);
                login.WriteAscii(_password);
                login.WriteUInt8(_locale);
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
            List<SilkroadServer> servers = new List<SilkroadServer>();

            while (packet.ReadUInt8() == 1)
            {
                byte farmId = packet.ReadUInt8();
                string farmName = packet.ReadAscii();
            }

            while (packet.ReadUInt8() == 1)
            {
                SilkroadServer silkroadServer = new SilkroadServer(
                    packet.ReadUInt16(),
                    packet.ReadAscii(),
                    (ServerCapacity) packet.ReadUInt8(),
                    packet.ReadUInt8() == 1
                );

                servers.Add(silkroadServer);
            }
            
            Application.Run(new Serverlist(servers, server));
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

                    Agent agent = new Agent(agentIp, agentPort, _locale, sessionId);
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

                List<CharSelect> chars = new List<CharSelect>();

                for (int i = 0; i < charCount; i++)
                {
                    packet.ReadUInt32();
                    string name = packet.ReadAscii();

                    if (agent.Locale == Locale.SRO_TR_Official_GameGami)
                    {
                        packet.ReadUInt16();
                    }

                    packet.ReadUInt8();
                    byte level = packet.ReadUInt8();
                    packet.ReadUInt64();
                    packet.ReadUInt16();
                    packet.ReadUInt16();
                    packet.ReadUInt16();

                    if (agent.Locale == Locale.SRO_TR_Official_GameGami)
                    {
                        packet.ReadUInt32();
                    }

                    packet.ReadUInt32();
                    packet.ReadUInt32();

                    if (agent.Locale == Locale.SRO_TR_Official_GameGami)
                    {
                        packet.ReadUInt16();
                    }

                    bool deleting = packet.ReadUInt8() == 1;

                    if (agent.Locale == Locale.SRO_TR_Official_GameGami)
                    {
                        packet.ReadUInt32();
                    }

                    CharSelect character = new CharSelect(name, level, deleting);

                    if (deleting)
                    {
                        uint minutes = packet.ReadUInt32();
                        character.DeletionTime = DateTime.Now.AddMinutes(minutes);
                    }

                    chars.Add(character);
                }

                Application.Run(new CharacterSelect(chars, server));
            }
        }

        [PacketHandler(Opcodes.Agent.Response.CHAR_DATA_CHUNK)]
        public void GameJoined(Server server, Packet packet)
        {
            server.Log("Successfully joined the game");
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
                chatMessage.SenderId = senderID;
            }
            else
            {
                string senderName = packet.ReadAscii();
                chatMessage.SenderName = senderName;
            }

            string message = packet.ReadUnicode();
            chatMessage.Message = message;
                            
            Program.Gui.AddChatMessage(chatMessage.ToString());
        }
    }
}