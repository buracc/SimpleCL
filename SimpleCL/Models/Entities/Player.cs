using System.Collections.Generic;
using SimpleCL.Enums.Commons;
using SimpleCL.Interaction;
using SimpleCL.Models.Character;
using SimpleCL.Models.Items;
using SimpleCL.Models.Skills;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Entities
{
    public class Player : Actor, ITargetable
    {
        public readonly List<InventoryItem> InventoryItems = new();
        public Player(uint id) : base(id)
        {
        }

        public bool IsWearingJobSuit()
        {
            return InventoryItems.Exists(item => item.IsEquipment() && item.TypeId3 == 7);
        }

        public void Attack(Skill skill)
        {
            var attackPacket = new Packet(Opcodes.Agent.Request.CHAR_ACTION);
            attackPacket.WriteByte(1);
            if (skill.Id == 1)
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
            var selectTarget = new Packet(Opcodes.Agent.Request.ENTITY_SELECT_OBJECT);
            selectTarget.WriteUInt(Uid);
            InteractionQueue.PacketQueue.Enqueue(selectTarget);
            InteractionQueue.PacketQueue.Enqueue(attackPacket);
        }

        public void Trace()
        {
            var actionPacket = new Packet(Opcodes.Agent.Request.CHAR_ACTION);
            actionPacket.WriteByte(1);
            actionPacket.WriteByte(3);
            actionPacket.WriteByte(1);
            actionPacket.WriteUInt(Uid);
            LocalPlayer.Get.Tracing = true;
            InteractionQueue.PacketQueue.Enqueue(actionPacket);
        }
    }
}