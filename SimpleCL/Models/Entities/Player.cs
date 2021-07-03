﻿using System.Collections.Generic;
using SimpleCL.Enums.Commons;
using SimpleCL.Models.Items;
using SimpleCL.Models.Skills;
using SimpleCL.SilkroadSecurityApi;

namespace SimpleCL.Models.Entities
{
    public class Player : Actor, ITargetable
    {
        public readonly List<InventoryItem> InventoryItems = new List<InventoryItem>();
        public Player(uint id) : base(id)
        {
        }

        public bool IsWearingJobSuit()
        {
            return InventoryItems.Exists(item => item.IsEquipment() && item.TypeId3 == 7);
        }

        public void Cast(Skill skill)
        {
            Packet attackPacket = new Packet(Opcodes.Agent.Request.CHAR_ACTION);
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
        }
    }
}