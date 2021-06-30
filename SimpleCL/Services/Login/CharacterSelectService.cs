using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SilkroadSecurityApi;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Server;
using SimpleCL.Models.Character;
using SimpleCL.Network;
using SimpleCL.Util;
using SimpleCL.Util.Extension;

namespace SimpleCL.Services.Login
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
            byte action = packet.ReadByte();
            bool succeeded = packet.ReadByte() == 1;

            if (action != 2 || !succeeded)
            {
                return;
            }
            
            byte charCount = packet.ReadByte();

            List<Character> chars = new List<Character>();
            charCount.Repeat(i =>
            {
                packet.ReadUInt();
                string name = packet.ReadAscii();
                if (_silkroadServer.Locale.IsInternational())
                {
                    string jobName = packet.ReadAscii();
                }

                packet.ReadByte();
                byte level = packet.ReadByte();
                packet.ReadULong();
                packet.ReadUShort();
                packet.ReadUShort();
                packet.ReadUShort();

                if (_silkroadServer.Locale.IsInternational())
                {
                    packet.ReadUInt();
                }

                uint hp = packet.ReadUInt();
                uint mp = packet.ReadUInt();

                if (_silkroadServer.Locale.IsInternational())
                {
                    packet.ReadUShort();
                }

                bool deleting = packet.ReadByte() == 1;

                if (_silkroadServer.Locale.IsInternational())
                {
                    packet.ReadUInt();
                }

                Character character = new Character(name, level, deleting);

                character.Hp = hp;
                character.Mp = mp;

                if (deleting)
                {
                    uint minutes = packet.ReadUInt();
                    character.DeletionTime = DateTime.Now.AddMinutes(minutes);
                }

                byte guildMemberClass = packet.ReadByte();

                bool guildRenameRequired = packet.ReadByte() == 1;
                if (guildRenameRequired)
                {
                    string guildName = packet.ReadAscii();
                }

                byte academyMemberClass = packet.ReadByte();
                byte itemCount = packet.ReadByte();

                itemCount.Repeat(j =>
                {
                    uint refItemId = packet.ReadUInt();
                    byte plus = packet.ReadByte();
                });

                byte avatarItemCount = packet.ReadByte();
                avatarItemCount.Repeat(j =>
                {
                    uint refItemId = packet.ReadUInt();
                    byte plus = packet.ReadByte();
                });

                chars.Add(character);
            });

            Application.Run(new Ui.CharacterSelection(chars, server));
        }
    }
}