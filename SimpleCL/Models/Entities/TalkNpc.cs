using System;
using SimpleCL.Enums;
using SimpleCL.Enums.Commons;
using SimpleCL.Interaction;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Entities
{
    public class TalkNpc : Npc
    {
        public string MapIcon
        {
            get
            {
                return ServerName switch
                {
                    _ when ServerName.Contains(Constants.Strings.Warehouse) => Constants.Strings.GuildStorageIcon,
                    _ when ServerName.Contains(Constants.Strings.Accessory) => Constants.Strings.EtcIcon,
                    _ when ServerName.Contains(Constants.Strings.Smith) => Constants.Strings.SmithIcon,
                    _ when ServerName.Contains(Constants.Strings.Potion) => Constants.Strings.PotionIcon,
                    _ when ServerName.Contains(Constants.Strings.Armor) => Constants.Strings.ArmorIcon,
                    _ when ServerName.Contains(Constants.Strings.Horse) => Constants.Strings.StableIcon,
                    _ => Constants.Strings.NpcIcon
                };
            }
        }

        public TalkNpc(uint id) : base(id)
        {
        }

        public void Talk()
        {
            var talkPacket = new Packet(Opcode.Agent.Request.ENTITY_SELECT_OBJECT);
            talkPacket.WriteUInt(Uid);
            talkPacket.Send();
        }
    }
}