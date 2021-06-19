using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Timers;
using SilkroadSecurityApi;
using SimpleCL.Model.Game;
using SimpleCL.Network.Enums;
using SimpleCL.Util;
using Timer = System.Timers.Timer;

namespace SimpleCL.Network
{
    public class Agent
    {
        private readonly Security _security = new Security();
        private readonly TransferBuffer _recvBuffer = new TransferBuffer(0x1000, 0, 0);
        private readonly Socket _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private string Ip;
        private ushort Port;
        private byte Locale;
        private uint SessionId;

        private Timer _timer = new Timer(5000);

        public Agent(string ip, ushort port, byte locale, uint sessionId)
        {
            Ip = ip;
            Port = port;
            Locale = locale;
            SessionId = sessionId;
        }

        public void Start()
        {
            try
            {
                _socket.Connect(Ip, Port);
                Console.WriteLine("Connected to agent " + Ip + ":" + Port);
            }
            catch (SocketException e)
            {
                Console.WriteLine("Unable to connect to agent");
                Console.WriteLine(e);
            }

            Thread agLoop = new Thread(Loop);
            agLoop.Start();
            _socket.Blocking = false;
            _socket.NoDelay = true;
        }
        
        public void Loop()
        {
            _timer.Elapsed += HeartBeat;

            while (true)
            {
                SocketError success;

                try
                {
                    _recvBuffer.Size = _socket.Receive(_recvBuffer.Buffer, 0, _recvBuffer.Buffer.Length,
                        SocketFlags.None,
                        out success);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                if (success != SocketError.Success && success != SocketError.WouldBlock)
                {
                    return;
                }

                if (_recvBuffer.Size < 0)
                {
                    return;
                }

                try
                {
                    _security.Recv(_recvBuffer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                List<Packet> incomingPackets = _security.TransferIncoming();
                if (incomingPackets == null)
                {
                    continue;
                }

                foreach (var packet in incomingPackets)
                {
                    // Console.WriteLine(packet.Opcode.ToString("X"));
                    // Console.WriteLine(Utility.HexDump(packet.GetBytes()));

                    switch (packet.Opcode)
                    {
                        case Opcodes.HANDSHAKE:
                        case Opcodes.HANDSHAKE_ACCEPT:
                            continue;
                        
                        case Opcodes.IDENTITY:
                            if (!_timer.Enabled)
                            {
                                _timer.Start();
                            }
                            
                            if (packet.ReadAscii().Equals("AgentServer"))
                            {
                                Packet login = new Packet(Opcodes.Agent.Request.AUTH, true);
                                login.WriteUInt32(SessionId);
                                login.WriteAscii(Credentials.Username);
                                login.WriteAscii(Credentials.Password);
                                login.WriteUInt8(Locale);
                                login.WriteUInt8Array(NetworkUtils.GetMacAddressBytes());
                            
                                _security.Send(login);
                            }
                            
                            break;
                        
                        case Opcodes.Agent.Response.AUTH:
                            bool result = packet.ReadUInt8() == 1;
                            if (result)
                            {
                                AuthErrorCode error = (AuthErrorCode) packet.ReadUInt8();
                                switch (error)
                                {
                                    case AuthErrorCode.IpLimit:
                                        Log("IP limit exceeded.");    
                                        return;
                                    
                                    case AuthErrorCode.ServerFull:
                                        Log("Server is full.");
                                        return;
                                }
                                
                                Packet charSelect = new Packet(Opcodes.Agent.Request.CHARACTER_SELECTION_ACTION);
                                charSelect.WriteUInt8(2);
                                _security.Send(charSelect);
                            }
                            
                            break;
                        
                        case Opcodes.Agent.Response.CHARACTER_SELECTION_ACTION:
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

                                    if (Locale == (byte) Enums.Locale.SRO_TR_Official_GameGami)
                                    {
                                        packet.ReadUInt16();
                                    }

                                    packet.ReadUInt8();
                                    byte level = packet.ReadUInt8();
                                    packet.ReadUInt64();
                                    packet.ReadUInt16();
                                    packet.ReadUInt16();
                                    packet.ReadUInt16();
                                    
                                    if (Locale == (byte) Enums.Locale.SRO_TR_Official_GameGami)
                                    {
                                        packet.ReadUInt32();
                                    }

                                    packet.ReadUInt32();
                                    packet.ReadUInt32();
                                    
                                    if (Locale == (byte) Enums.Locale.SRO_TR_Official_GameGami)
                                    {
                                        packet.ReadUInt16();
                                    }

                                    bool deleting = packet.ReadUInt8() == 1;
                                    
                                    if (Locale == (byte) Enums.Locale.SRO_TR_Official_GameGami)
                                    {
                                        packet.ReadUInt32();
                                    }
                                    
                                    CharSelect character = new CharSelect(name, level, deleting);

                                    if (deleting)
                                    {
                                        uint minutes = packet.ReadUInt32();
                                        character.DeletionTime = DateTime.Now.AddMinutes(minutes);;
                                    }

                                    characters[character.Name] = character;
                                }

                                Log("Characters:");
                                foreach (var c in characters)
                                {
                                    Log(c.Value.ToString());
                                }

                                Packet characterJoin = new Packet(Opcodes.Agent.Request.CHARACTER_SELECTION_JOIN);
                                characterJoin.WriteAscii(Credentials.CharName);
                                _security.Send(characterJoin);
                            }
                            
                            break;
                        
                        case Opcodes.Agent.Response.CHAR_DATA_CHUNK:
                            Log("Successfully joined the game");
                            break;
                        
                        case Opcodes.Agent.Response.CHAR_CELESTIAL_POSITION:
                            _security.Send(new Packet(Opcodes.Agent.Request.GAME_READY));
                            break;
                        
                        case Opcodes.Agent.Response.CHAT_UPDATE:
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
                            
                            Console.WriteLine(chatMessage.ToString());
                            break;
                    }
                }

                List<KeyValuePair<TransferBuffer, Packet>> outgoing = _security.TransferOutgoing();
                if (outgoing != null)
                {
                    foreach (var pair in outgoing)
                    {
                        TransferBuffer buffer = pair.Key;
                        success = SocketError.Success;

                        while (buffer.Offset != buffer.Size)
                        {
                            int n = _socket.Send(buffer.Buffer, buffer.Offset, buffer.Size - buffer.Offset,
                                SocketFlags.None, out success);
                            if (success != SocketError.Success && success != SocketError.WouldBlock)
                            {
                                break;
                            }

                            buffer.Offset += n;
                            Thread.Sleep(1);
                        }

                        if (success != SocketError.Success)
                        {
                            break;
                        }
                    }

                    if (success != SocketError.Success)
                    {
                        break;
                    }
                }

                Thread.Sleep(1);
            }
        }

        public void HeartBeat(Object source, ElapsedEventArgs e)
        { 
            _security.Send(new Packet(Opcodes.HEARTBEAT));
        }

        public void Log(string message)
        {
            Console.WriteLine("[Agent] " + message);
        }
    }
}