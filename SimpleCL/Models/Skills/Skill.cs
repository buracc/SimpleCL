using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCL.Database;
using SimpleCL.Enums;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Items.Type;
using SimpleCL.Enums.Skills;
using SimpleCL.Interaction;
using SimpleCL.Models.Exceptions;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Skills
{
    public class Skill
    {
        public long CastTimeStamp = Environment.TickCount;

        public readonly uint Id;
        public readonly ushort GroupId;
        public readonly string ServerName;
        public readonly string Name;
        public readonly uint CastTime;
        public readonly uint Cooldown;
        public readonly int Duration;
        public readonly ushort MasteryId;
        public readonly sbyte SkillGroup;
        public readonly sbyte SkillGroupIndex;
        public readonly uint SpRequired;
        public readonly uint MpRequired;
        public readonly ushort Level;
        public readonly string IconPath;
        public readonly string Description;
        public readonly List<SkillData.Attribute> Attributes;
        public readonly ushort RequiredGroupId1;
        public readonly ushort RequiredGroupId2;
        public readonly ushort RequiredGroupId3;
        public readonly ushort RequiredLevel1;
        public readonly ushort RequiredLevel2;
        public readonly ushort RequiredLevel3;
        public readonly EquipmentData.SubType.Weapon RequiredWeapon1;
        public readonly EquipmentData.SubType.Weapon RequiredWeapon2;
        public readonly bool Targeted;
        public readonly ushort Range;

        public Skill(uint id)
        {
            var data = GameDatabase.Get.GetSkill(id);

            if (data == null)
            {
                throw new EntityParseException(id);
            }

            Id = uint.Parse(data[Constants.Strings.Id]);
            GroupId = ushort.Parse(data[Constants.Strings.GroupId]);
            ServerName = data[Constants.Strings.ServerName];
            Name = data[Constants.Strings.Name];
            CastTime = uint.Parse(data[Constants.Strings.CastTime]);
            Cooldown = uint.Parse(data[Constants.Strings.Cooldown]);
            Duration = int.Parse(data[Constants.Strings.Duration]);
            MasteryId = ushort.Parse(data[Constants.Strings.Mastery]);
            SkillGroup = sbyte.Parse(data[Constants.Strings.SkillGroup]);
            SkillGroupIndex = sbyte.Parse(data[Constants.Strings.SkillGroupIndex]);
            SpRequired = uint.Parse(data[Constants.Strings.Sp]);
            MpRequired = uint.Parse(data[Constants.Strings.Mp]);
            Level = ushort.Parse(data[Constants.Strings.Level]);
            IconPath = data[Constants.Strings.Icon];
            Description = data[Constants.Strings.Description];
            Attributes = Array.ConvertAll(data[Constants.Strings.Attributes].Split(','), uint.Parse)
                .Select(x => (SkillData.Attribute) x).ToList();
            RequiredGroupId1 = ushort.Parse(data[Constants.Strings.RequiredLearn1]);
            RequiredGroupId2 = ushort.Parse(data[Constants.Strings.RequiredLearn2]);
            RequiredGroupId3 = ushort.Parse(data[Constants.Strings.RequiredLearn3]);
            RequiredLevel1 = ushort.Parse(data[Constants.Strings.RequiredLevel1]);
            RequiredLevel2 = ushort.Parse(data[Constants.Strings.RequiredLevel2]);
            RequiredLevel3 = ushort.Parse(data[Constants.Strings.RequiredLevel3]);
            RequiredWeapon1 = (EquipmentData.SubType.Weapon) byte.Parse(data[Constants.Strings.Weapon1]);
            RequiredWeapon2 = (EquipmentData.SubType.Weapon) byte.Parse(data[Constants.Strings.Weapon2]);
            Targeted = byte.Parse(data[Constants.Strings.TargetRequired]) == 1;
            Range = ushort.Parse(data[Constants.Strings.Range]);
        }

        public bool IsOnCooldown()
        {
            return Environment.TickCount - CastTimeStamp < Cooldown;
        }

        public void StartCooldownTimer()
        {
            CastTimeStamp = Environment.TickCount;
        }

        public void Cast()
        {
            var actionPacket = new Packet(Opcode.Agent.Request.CHAR_ACTION);
            actionPacket.WriteByte(1);
            actionPacket.WriteByte(4);
            actionPacket.WriteUInt(Id);
            actionPacket.WriteByte(0);
            InteractionQueue.PacketQueue.Enqueue(actionPacket);
        }
        
        public void Cancel()
        {
            var actionPacket = new Packet(Opcode.Agent.Request.CHAR_ACTION);
            actionPacket.WriteByte(1);
            actionPacket.WriteByte(5);
            actionPacket.WriteUInt(Id);
            actionPacket.WriteByte(0);
            InteractionQueue.PacketQueue.Enqueue(actionPacket);
        }
        
        public bool IsResSkill()
        {
            return Attributes.Exists(x => x == SkillData.Attribute.Resurrection);
        }
        
        public bool IsAttackSkill()
        {
            return Attributes.Exists(x => x == SkillData.Attribute.Attack);
        }

        public override string ToString()
        {
            return Name;
        }

        public enum CastType : byte
        {
            Buff = 0,
            Attack = 2
        }

        [Flags]
        public enum DamageEffect : byte
        {
            None = 0,
            KnockBack = 1,
            Block = 2,
            Position = 4,
            Cancel = 8
        }

        [Flags]
        public enum DamageType : byte
        {
            None = 0,
            Normal = 1,
            Critical = 2,
            Status = 4
        }
    }
}