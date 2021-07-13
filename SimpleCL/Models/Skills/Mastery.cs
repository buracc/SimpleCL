using System;
using SimpleCL.Database;
using SimpleCL.Enums;
using SimpleCL.Enums.Items.Type;

namespace SimpleCL.Models.Skills
{
    public class Mastery
    {
        public readonly uint Id;
        public readonly sbyte GroupIndex;
        public readonly string Name;
        public readonly string Description;
        public readonly MasteryType Type;
        public readonly EquipmentType.Weapon[] RequiredWeapons;
        public readonly string IconPath;
        public ushort Level { get; set; }

        public Mastery(uint id)
        {
            var data = GameDatabase.Get.GetMastery(id);

            if (data == null)
            {
                return;
            }

            Id = uint.Parse(data["id"]);
            GroupIndex = sbyte.Parse(data["group_index"]);
            Name = data[Constants.Strings.Name];
            Description = data[Constants.Strings.Name];
            Type = (MasteryType) Enum.Parse(typeof(MasteryType), data["type"]);
            RequiredWeapons = Array.ConvertAll(data["weapon"].Split(','),
                x => (EquipmentType.Weapon) byte.Parse(x));
            IconPath = data["icon"];
        }

        public enum MasteryType
        {
            Weapon,
            Force,
            Recovery,
            Melee,
            Caster,
            Buff,
            Job
        }
    }
}