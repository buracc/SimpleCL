using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SilkroadSecurityApi;
using SimpleCL.Database;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Login.Error;
using SimpleCL.Enums.Server;
using SimpleCL.Models.Server;
using SimpleCL.Network;
using SimpleCL.Services.Game.Chatter;
using SimpleCL.Services.Game.Entities;
using SimpleCL.Ui;
using SimpleCL.Util;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Login
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
                identity.WriteByte(_silkroadServer.Locale);
                identity.WriteAscii("SR_Client");
                identity.WriteUInt(GameDatabase.Get.GetGameVersion());
                server.Inject(identity);
                return;
            }

            if (server is Agent)
            {
                Packet login = new Packet(Opcodes.Agent.Request.AUTH, true);
                login.WriteUInt(((Agent) server).SessionId);
                login.WriteAscii(_username);
                login.WriteAscii(_password);
                login.WriteByte(_silkroadServer.Locale);
                login.WriteByteArray(NetworkUtils.GetMacAddressBytes());

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

            while (packet.ReadByte() == 1)
            {
                byte farmId = packet.ReadByte();
                string farmName = packet.ReadAscii();
            }

            while (packet.ReadByte() == 1)
            {
                GameServer gameServer = new GameServer(
                    packet.ReadUShort(),
                    packet.ReadAscii(),
                    (ServerCapacity) packet.ReadByte(),
                    packet.ReadByte() == 1
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
            packet.ReadByte();
            byte passcodeResult = packet.ReadByte();
            if (passcodeResult == 2)
            {
                byte attempts = packet.ReadByte();
                Application.Run(new PasscodeEnter(server, "Invalid passcode [" + attempts + "/" + 3 + "]"));
                server.Log("Invalid passcode. Attempts: [" + attempts + "/" + 3 + "]");
            }
        }

        [PacketHandler(Opcodes.Gateway.Response.AGENT_AUTH)]
        public void AgentAuth(Server server, Packet packet)
        {
            byte result = packet.ReadByte();
            switch (result)
            {
                case 1:
                    uint sessionId = packet.ReadUInt();
                    string agentIp = packet.ReadAscii();
                    ushort agentPort = packet.ReadUShort();

                    Agent agent = new Agent(agentIp, agentPort, sessionId);
                    
                    agent.RegisterService(this);
                    agent.RegisterService(new ChatService());
                    agent.RegisterService(new CharacterSelectService(_silkroadServer));
                    agent.RegisterService(new LocalPlayerService(_silkroadServer, (Gateway) server));
                    agent.RegisterService(new SpawnService());
                    agent.RegisterService(new StatusService());
                    agent.RegisterService(new MovementService());
                    
                    // agent.Debug = true;
                    agent.Start();
                    break;

                case 2:
                    LoginErrorCode errorCode = (LoginErrorCode) packet.ReadByte();

                    switch (errorCode)
                    {
                        case LoginErrorCode.InvalidCredentials:
                            uint maxAttempts = packet.ReadUInt();
                            uint currentAttempts = packet.ReadUInt();
                            server.Log("Invalid credentials. Attempts: [" + currentAttempts +
                                       "/" + maxAttempts + "]");
                            break;

                        case LoginErrorCode.Blocked:
                            LoginBlockType blockType = (LoginBlockType) packet.ReadByte();

                            switch (blockType)
                            {
                                case LoginBlockType.Punishment:
                                    string reason = packet.ReadAscii();
                                    DateTime endDate = new DateTime(
                                        packet.ReadUShort(),
                                        packet.ReadUShort(),
                                        packet.ReadUShort(),
                                        packet.ReadUShort(),
                                        packet.ReadUShort(),
                                        packet.ReadUShort(),
                                        packet.ReadUShort()
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
                        case LoginErrorCode.LoginQueue:
                            server.Log("Login queue");
                            return;
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

        [PacketHandler(Opcodes.Gateway.Response.QUEUE_POSITION)]
        public void LoginQueuePosition(Server server, Packet packet)
        {
            packet.ReadByte();
            ushort playersInQueue = packet.ReadUShort();
            packet.ReadUInt();
            ushort currentPosition = packet.ReadUShort();
            
            server.Log("Queue position: [" + currentPosition + "/" + playersInQueue + "]");
        }

        [PacketHandler(Opcodes.Agent.Response.AUTH)]
        public void EnterCharacterSelect(Server server, Packet packet)
        {
            byte result = packet.ReadByte();
            if (result == 1)
            {
                Packet charSelect = new Packet(Opcodes.Agent.Request.CHARACTER_SELECTION_ACTION);
                charSelect.WriteByte(2);
                server.Inject(charSelect);
                return;
            }

            if (result == 2)
            {
                byte errorCode = packet.ReadByte();
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
    }
}