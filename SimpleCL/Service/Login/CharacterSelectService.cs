using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SilkroadSecurityApi;
using SimpleCL.Enums.Common;
using SimpleCL.Enums.Server;
using SimpleCL.Model.Character;
using SimpleCL.Network;

namespace SimpleCL.Service.Login
{
    public class CharacterSelectService : Service
    {
        private readonly SilkroadServer _silkroadServer;

        public CharacterSelectService(SilkroadServer silkroadServer)
        {
            _silkroadServer = silkroadServer;
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

                Application.Run(new Ui.CharacterSelection(chars, server));
            }
        }
    }
}