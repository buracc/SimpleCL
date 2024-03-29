﻿using System.Collections.Generic;
using SimpleCL.Enums.Commons;
using SimpleCL.Models.Character;
using SimpleCL.Models.Entities.Exchange;
using SimpleCL.Models.Items;
using SimpleCL.Models.Items.Equipables;
using SimpleCL.Models.Skills;
using SimpleCL.SecurityApi;
using SimpleCL.Util.Extension;

namespace SimpleCL.Models.Entities
{
    public class Player : Actor, ITargetable
    {
        public readonly List<InventoryItem> Inventory = new();
        public Interaction InteractionType { get; set; }
        public Stall Stall { get; set; }
        
        public Player(uint id) : base(id)
        {
        }

        public bool IsWearingJobSuit()
        {
            return Inventory.Exists(item => item is Equipment {SlotType: Equipment.EquipmentSlot.Cape});
        }

        public void Attack(Skill skill)
        {
            var attackPacket = new Packet(Opcode.Agent.Request.CHAR_ACTION);
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
            
            var selectTarget = new Packet(Opcode.Agent.Request.ENTITY_SELECT_OBJECT);
            selectTarget.WriteUInt(Uid);
            
            selectTarget.Send();
            attackPacket.Send();
        }

        public void Trace()
        {
            var actionPacket = new Packet(Opcode.Agent.Request.CHAR_ACTION);
            actionPacket.WriteByte(1);
            actionPacket.WriteByte(3);
            actionPacket.WriteByte(1);
            actionPacket.WriteUInt(Uid);
            LocalPlayer.Get.Tracing = true;
            actionPacket.Send();
        }

        public new void Dispose()
        {
            base.Dispose();
            
            Inventory?.DisposeAll();
            Inventory?.Clear();
            
            Stall?.Dispose();
        }
        
        public enum Interaction : byte
        {
            None = 0,
            OnExchange = 2,
            OnStall = 3,
            OnShop = 5
        }
    }
}