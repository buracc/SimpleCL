using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SimpleCL.Database;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Login.Error;
using SimpleCL.Enums.Server;
using SimpleCL.Models.Server;
using SimpleCL.Network;
using SimpleCL.SecurityApi;
using SimpleCL.Services.Game;
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
        private readonly Gateway _gateway;

        public LoginService(string username, string password, SilkroadServer silkroadServer, Gateway gateway)
        {
            _username = username;
            _password = password;
            _silkroadServer = silkroadServer;
            _gateway = gateway;
        }

        #region SendIdentity

        [PacketHandler(Opcode.IDENTITY)]
        public void SendIdentity(Server server, Packet packet)
        {
            switch (server)
            {
                case Gateway:
                {
                    var identity = new Packet(Opcode.Gateway.Request.PATCH, true);
                    identity.WriteByte(_silkroadServer.Locale);
                    identity.WriteAscii("SR_Client");
                    identity.WriteUInt(GameDatabase.Get.GetGameVersion());
                    server.Inject(identity);
                    return;
                }
                
                case Agent agent:
                {
                    var login = new Packet(Opcode.Agent.Request.AUTH, true);
                    login.WriteUInt(agent.SessionId);
                    login.WriteAscii(_username);
                    login.WriteAscii(_password);
                    login.WriteByte(_silkroadServer.Locale);
                    login.WriteByteArray(NetworkUtils.GetMacAddressBytes());

                    agent.Inject(login);
                    break;
                }
            }
        }

        #endregion

        #region RequestServerlist

        [PacketHandler(Opcode.Gateway.Response.PATCH)]
        public void RequestServerlist(Server server, Packet packet)
        {
            var status = packet.ReadByte();
            if (status == 2)
            {
                server.Log("The game requires an update");
                server.Disconnect();
                return;
            }
            
            if (server.IsProxying())
            {
                return;
            }
            
            server.Inject(new Packet(Opcode.Gateway.Request.SERVERLIST, true));
        }

        #endregion

        #region SendLogin

        [PacketHandler(Opcode.Gateway.Response.SERVERLIST)]
        public void SendLogin(Server server, Packet packet)
        {
            var servers = new List<GameServer>();

            while (packet.ReadByte() == 1)
            {
                var farmId = packet.ReadByte();
                var farmName = packet.ReadAscii();
            }

            while (packet.ReadByte() == 1)
            {
                var gameServer = new GameServer(
                    packet.ReadUShort(),
                    packet.ReadAscii(),
                    (ServerCapacity) packet.ReadByte(),
                    packet.ReadByte() == 1
                );

                servers.Add(gameServer);
            }

            new Serverlist(_username, _password, servers, server).ShowDialog();
        }

        #endregion

        #region SendPasscode

        [PacketHandler(Opcode.Gateway.Response.LOGIN2)]
        public void SendPasscode(Server server, Packet packet)
        {
            new PasscodeEnter(server).ShowDialog();
        }

        #endregion

        #region PasscodeResponse

        [PacketHandler(Opcode.Gateway.Response.PASSCODE)]
        public void PasscodeResponse(Server server, Packet packet)
        {
            packet.ReadByte();
            var passcodeResult = packet.ReadByte();
            if (passcodeResult != 2)
            {
                return;
            }
            
            var attempts = packet.ReadByte();
            new PasscodeEnter(server, "Invalid passcode [" + attempts + "/" + 3 + "]").ShowDialog();
            server.Log("Invalid passcode. Attempts: [" + attempts + "/" + 3 + "]");
        }

        #endregion

        #region AgentAuth

        [PacketHandler(Opcode.Gateway.Response.AGENT_AUTH)]
        public void AgentAuth(Server server, Packet packet)
        {
            var result = packet.ReadByte();
            switch (result)
            {
                case 1:
                    var sessionId = packet.ReadUInt();
                    var agentIp = packet.ReadAscii();
                    var agentPort = packet.ReadUShort();

                    var agent = new Agent(agentIp, agentPort, sessionId, _gateway);
                    
                    agent.RegisterService(this);
                    
                    agent.RegisterService(new ChatService());
                    agent.RegisterService(new CharacterSelectService(_silkroadServer));
                    agent.RegisterService(new LocalPlayerService(_silkroadServer, (Gateway) server));
                    agent.RegisterService(new EntitySpawnService());
                    agent.RegisterService(new EntityHealthService());
                    agent.RegisterService(new EntityMovementService());
                    agent.RegisterService(new EntitySkillService());
                    agent.RegisterService(new EntityStateService());
                    agent.RegisterService(new StallService(_silkroadServer));
                    agent.RegisterService(new NpcService());
                    agent.RegisterService(new InventoryService());
                    
                    // agent.Debug = true;
                    // agent.Start();
                    break;

                case 2:
                    var errorCode = (LoginErrorCode) packet.ReadByte();

                    switch (errorCode)
                    {
                        case LoginErrorCode.InvalidCredentials:
                            var maxAttempts = packet.ReadUInt();
                            var currentAttempts = packet.ReadUInt();
                            server.Log("Invalid credentials. Attempts: [" + currentAttempts +
                                       "/" + maxAttempts + "]");
                            break;

                        case LoginErrorCode.Blocked:
                            var blockType = (LoginBlockType) packet.ReadByte();

                            switch (blockType)
                            {
                                case LoginBlockType.Punishment:
                                    var reason = packet.ReadAscii();
                                    var endDate = new DateTime(
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
                        
                        case LoginErrorCode.PasswordExpired:
                            server.Log("Password expired. Set a new password.");
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

        #endregion

        #region LoginQueue

        [PacketHandler(Opcode.Gateway.Response.QUEUE_POSITION)]
        public void LoginQueuePosition(Server server, Packet packet)
        {
            packet.ReadByte();
            var playersInQueue = packet.ReadUShort();
            packet.ReadUInt();
            var currentPosition = packet.ReadUShort();
            
            server.Log("Queue position: [" + currentPosition + "/" + playersInQueue + "]");
        }

        #endregion

        #region RequestCharSelect

        [PacketHandler(Opcode.Agent.Response.AUTH)]
        public void EnterCharacterSelect(Server server, Packet packet)
        {
            if (server.IsProxying())
            {
                return;
            }
            
            var result = packet.ReadByte();
            switch (result)
            {
                case 1:
                {
                    var charSelect = new Packet(Opcode.Agent.Request.CHARACTER_SELECTION_ACTION);
                    charSelect.WriteByte(2);
                    server.Inject(charSelect);
                    return;
                }
                
                case 2:
                {
                    var errorCode = packet.ReadByte();
                    var error = (AuthErrorCode) errorCode;
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
                        
                        case AuthErrorCode.LogoutProcess:
                            server.Log("Account is in the process of logging out.");
                            break;
                    
                        default:
                            server.Log("Unhandled auth error code: " + error);
                            break;
                    }
                
                    server.Disconnect();
                    break;
                }
            }
        }

        #endregion
    }
}