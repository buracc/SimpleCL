using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SimpleCL.SilkroadSecurityApi;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Server;
using SimpleCL.Models.Character;
using SimpleCL.Network;
using SimpleCL.Ui;
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

        #region CharSelect

        [PacketHandler(Opcodes.Agent.Response.CHARACTER_SELECTION_ACTION)]
        public void SelectCharacter(Server server, Packet packet)
        {
            var action = packet.ReadByte();
            var succeeded = packet.ReadByte() == 1;

            if (action != 2 || !succeeded)
            {
                return;
            }
            
            var charCount = packet.ReadByte();

            var chars = new List<Character>();
            charCount.Repeat(i =>
            {
                packet.ReadUInt();
                var name = packet.ReadAscii();
                if (_silkroadServer.Locale.IsInternational())
                {
                    var jobName = packet.ReadAscii();
                }

                packet.ReadByte();
                var level = packet.ReadByte();
                packet.ReadULong();
                packet.ReadUShort();
                packet.ReadUShort();
                packet.ReadUShort();

                if (_silkroadServer.Locale.IsInternational())
                {
                    packet.ReadUInt();
                }

                var hp = packet.ReadUInt();
                var mp = packet.ReadUInt();

                if (_silkroadServer.Locale.IsInternational())
                {
                    packet.ReadUShort();
                }

                var deleting = packet.ReadByte() == 1;

                if (_silkroadServer.Locale.IsInternational())
                {
                    packet.ReadUInt();
                }

                var character = new Character(name, level, deleting) {Hp = hp, Mp = mp};
                
                if (deleting)
                {
                    var minutes = packet.ReadUInt();
                    character.DeletionTime = DateTime.Now.AddMinutes(minutes);
                }

                var guildMemberClass = packet.ReadByte();

                var guildRenameRequired = packet.ReadByte() == 1;
                if (guildRenameRequired)
                {
                    var guildName = packet.ReadAscii();
                }

                var academyMemberClass = packet.ReadByte();
                var itemCount = packet.ReadByte();

                itemCount.Repeat(j =>
                {
                    var refItemId = packet.ReadUInt();
                    var plus = packet.ReadByte();
                });

                var avatarItemCount = packet.ReadByte();
                avatarItemCount.Repeat(j =>
                {
                    var refItemId = packet.ReadUInt();
                    var plus = packet.ReadByte();
                });

                chars.Add(character);
            });

            Application.Run(new CharacterSelection(chars, server));
        }

        #endregion
    }
}