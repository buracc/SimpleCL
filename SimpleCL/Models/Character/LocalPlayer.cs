using System.Collections.Generic;
using System.ComponentModel;
using SimpleCL.Database;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Items;
using SimpleCL.Models.Skills;

namespace SimpleCL.Models.Character
{
    public class LocalPlayer : Player
    {
        private LocalPlayer(uint id) : base(id)
        {
        }

        private static LocalPlayer _instance;
        public static LocalPlayer Get => _instance ??= new LocalPlayer(1907);

        private byte _level;
        private byte _jobLevel;
        public uint MaxHp { get; set; }
        public uint MaxMp { get; set; }
        public readonly List<Mastery> Masteries = new();
        public readonly BindingList<CharacterSkill> Skills = new();

        public byte Level
        {
            get => _level;
            set
            {
                _level = value;
                NextLevelExp = GameDatabase.Get.GetNextLevelExp(value);
            }
        }

        public string JobName { get; set; }
        public ulong NextLevelExp { get; private set; }

        public ulong JobNextLevelExp { get; private set; }

        public byte JobLevel
        {
            get => _jobLevel;
            set
            {
                _jobLevel = value;
                JobNextLevelExp = GameDatabase.Get.GetJobNextLevelExp(value);
            }
        }

        public ulong ExpGained { get; set; }
        public ulong JobExpGained { get; set; }
        public uint Skillpoints { get; set; }
        public ulong Gold { get; set; }

        public Dictionary<string, List<InventoryItem>> Inventories = new();

        public float GetAngleDegrees()
        {
            return Angle * 360f / ushort.MaxValue;
        }

        public double GetExpPercent()
        {
            return GetExpPercentDecimal() * 100;
        }

        public double GetJobExpPercent()
        {
            return GetJobExpPercentDecimal() * 100;
        }

        public double GetExpPercentDecimal()
        {
            return (double) ExpGained / NextLevelExp;
        }

        public double GetJobExpPercentDecimal()
        {
            return (double) JobExpGained / JobNextLevelExp;
        }
    }
}