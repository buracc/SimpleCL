using System;
using SimpleCL.Database;
using SimpleCL.Enums.Items.Type;

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
        public readonly string Icon;
        public readonly string Description;
        public readonly uint[] Attributes;
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
                Console.WriteLine("Couldn't parse skill with id: " + id);
                return;
            }

            Id = uint.Parse(data["id"]);
            GroupId = ushort.Parse(data["group_id"]);
            ServerName = data["servername"];
            Name = data["name"];
            CastTime = uint.Parse(data["casttime"]);
            Cooldown = uint.Parse(data["cooldown"]);
            Duration = int.Parse(data["duration"]);
            MasteryId = ushort.Parse(data["mastery"]);
            SkillGroup = sbyte.Parse(data["skillgroup"]);
            SkillGroupIndex = sbyte.Parse(data["skillgroup_index"]);
            SpRequired = uint.Parse(data["sp"]);
            MpRequired = uint.Parse(data["mp"]);
            Level = ushort.Parse(data["level"]);
            Icon = data["icon"];
            Description = data["description"];
            Attributes = Array.ConvertAll(data["attributes"].Split(','), uint.Parse);
            RequiredGroupId1 = ushort.Parse(data["requiredlearn_1"]);
            RequiredGroupId2 = ushort.Parse(data["requiredlearn_2"]);
            RequiredGroupId3 = ushort.Parse(data["requiredlearn_3"]);
            RequiredLevel1 = ushort.Parse(data["requiredlearnlevel_1"]);
            RequiredLevel2 = ushort.Parse(data["requiredlearnlevel_2"]);
            RequiredLevel3 = ushort.Parse(data["requiredlearnlevel_3"]);
            RequiredWeapon1 = (EquipmentData.SubType.Weapon) byte.Parse(data["weapon_1"]);
            RequiredWeapon2 = (EquipmentData.SubType.Weapon) byte.Parse(data["weapon_2"]);
            Targeted = byte.Parse(data["target_required"]) == 1;
            Range = ushort.Parse(data["range"]);
        }

        public bool IsOnCooldown()
        {
            return Environment.TickCount - CastTimeStamp < Cooldown;
        }

        public void StartCooldownTimer()
        {
            CastTimeStamp = Environment.TickCount;
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