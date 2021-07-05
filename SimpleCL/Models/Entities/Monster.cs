using SimpleCL.Enums.Commons;
using SimpleCL.Interaction;
using SimpleCL.Models.Skills;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Entities
{
    public class Monster : Npc, ITargetable
    {
        public Monster(uint id) : base(id)
        {
        }

        public bool IsThief()
        {
            return TypeId4 == 2;
        }
        
        public bool IsHunter()
        {
            return TypeId4 == 3;
        }
        
        public bool IsQuest()
        {
            return TypeId4 == 4;
        }
        
        public bool IsPandora()
        {
            return TypeId4 == 5;
        }
        
        public bool IsTraderCaravan()
        {
            return TypeId4 == 6;
        }
        
        public bool IsThiefCaravan()
        {
            return TypeId4 == 7;
        }
        
        public bool IsFlower()
        {
            return TypeId4 == 8;
        }
        
        public enum Type : byte
        {
            General = 0,
            Champion = 1,
            Giant = 4,
            Titan = 5,
            Strong = 6,
            Elite = 7,
            Unique = 8,
            PartyGeneral = 0x10,
            PartyChampion = 0x11,
            PartyGiant = 0x14,
            Event = 0xFF
        }

        public void Attack(Skill skill = null)
        {
            var attackPacket = new Packet(Opcodes.Agent.Request.CHAR_ACTION);
            attackPacket.WriteByte(1);
            if (skill == null || skill.Id == 1)
            {
                attackPacket.WriteByte(1);
            }
            else
            {
                attackPacket.WriteByte(4);
                attackPacket.WriteUInt(skill.Id);
            }
            
            attackPacket.WriteByte(1);
            attackPacket.WriteUInt(Uid);
            InteractionQueue.PacketQueue.Enqueue(attackPacket);
        }
    }
}