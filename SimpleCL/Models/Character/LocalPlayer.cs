using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using SimpleCL.Database;
using SimpleCL.Models.Entities;
using SimpleCL.Models.Items;
using SimpleCL.Models.Skills;

namespace SimpleCL.Models.Character
{
    public class LocalPlayer : Player
    {
        #region Constructor

        private LocalPlayer(uint id) : base(id)
        {
        }
        
        static LocalPlayer()
        {
        }

        public static LocalPlayer Get { get; } = new(1907);

        #endregion

        #region Members

        private string _jobName;
        private byte _level;
        private byte _jobLevel;
        private uint _maxHp;
        private uint _maxMp;
        private ulong _expGained;
        private ulong _jobExpgained;
        private uint _skillPoints;
        private ulong _gold;

        #endregion

        #region Properties

        public uint MaxHp
        {
            get => _maxHp;
            set
            {
                _maxHp = value;
                OnPropertyChanged(nameof(MaxHp));
            }
        }

        public uint MaxMp
        {
            get => _maxMp;
            set
            {
                _maxMp = value;
                OnPropertyChanged(nameof(MaxMp));
            }
        }

        public byte Level
        {
            get => _level;
            set
            {
                _level = value;
                NextLevelExp = GameDatabase.Get.GetNextLevelExp(value);
                OnPropertyChanged(nameof(Level));
            }
        }

        public string JobName
        {
            get => _jobName;
            set
            {
                _jobName = value;
                OnPropertyChanged(nameof(JobName));
            }
        }

        public ulong NextLevelExp { get; private set; }

        public ulong JobNextLevelExp { get; private set; }

        public byte JobLevel
        {
            get => _jobLevel;
            set
            {
                _jobLevel = value;
                JobNextLevelExp = GameDatabase.Get.GetJobNextLevelExp(_jobLevel);
                OnPropertyChanged(nameof(JobLevel));
            }
        }

        public ulong ExpGained
        {
            get => _expGained;
            set
            {
                _expGained = value;
                OnPropertyChanged(nameof(ExpPercent));
                OnPropertyChanged(nameof(ExpPercentString));
            }
        }

        public int ExpPercent => (int) (GetPercentDecimal(ExpGained, NextLevelExp) * 100);
        
        public ulong JobExpGained
        {
            get => _jobExpgained;
            set
            {
                _jobExpgained = value;
                OnPropertyChanged(nameof(JobExpPercent));
                OnPropertyChanged(nameof(JobExpPercentString));
            }
        }

        public int JobExpPercent => (int) (GetPercentDecimal(JobExpGained, JobNextLevelExp) * 100);
        

        public uint Skillpoints
        {
            get => _skillPoints;
            set
            {
                _skillPoints = value;
                OnPropertyChanged(nameof(SkillPointsString));
            }
        }

        public ulong Gold
        {
            get => _gold;
            set
            {
                _gold = value;
                OnPropertyChanged(nameof(GoldString));
            }
        }

        public bool Tracing { get; set; }

        public readonly List<Mastery> Masteries = new();
        public readonly BindingList<CharacterSkill> Skills = new();
        public readonly BindingList<InventoryItem> Inventory = new();
        public readonly BindingList<InventoryItem> EquipmentInventory = new();
        public readonly BindingList<InventoryItem> AvatarInventory = new();
        public readonly BindingList<InventoryItem> JobEquipmentInventory = new();

        #endregion
        
        #region UI Properties

        public string ExpPercentString => GetPercentString(GetPercentDecimal(ExpGained, NextLevelExp));
        public string JobExpPercentString => GetPercentString(GetPercentDecimal(JobExpGained, JobNextLevelExp));
        public string GoldString => Gold.ToString("N0");
        public string SkillPointsString => Skillpoints.ToString("N0");

        #endregion

        #region Utility methods

        public float GetAngleDegrees()
        {
            return Angle * 360f / ushort.MaxValue;
        }

        public double GetPercentDecimal(ulong gained, ulong nextExp)
        {
            if (gained == 0 || nextExp == 0)
            {
                return 0.0;
            }
            
            return (double) gained / nextExp;
        }

        public string GetPercentString(double gained)
        {
            return gained.ToString("P", CultureInfo.CurrentCulture);
        }

        #endregion
    }
}