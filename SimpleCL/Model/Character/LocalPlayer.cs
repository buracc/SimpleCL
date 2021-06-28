using System.Collections.Generic;
using SimpleCL.Database;
using SimpleCL.Model.Coord;
using SimpleCL.Model.Entity;
using SimpleCL.Model.Inventory;

namespace SimpleCL.Model.Character
{
    public class LocalPlayer : Player
    {
        private LocalPlayer(uint id) : base(id)
        { }
        
        private static LocalPlayer _instance;
        public static LocalPlayer Get => _instance ?? (_instance = new LocalPlayer(1907));

        public static void Refresh(uint refObjId)
        {
            _instance = new LocalPlayer(refObjId);
        }

        private ulong _nextLevelExp;
        private ulong _jobNextLevelExp;
        private byte _level;
        private byte _jobLevel;
        private ushort _angle;
        public uint Uid { get; set; }
        public uint Hp { get; set; }
        public uint Mp { get; set; }
        public uint MaxHp { get; set; }
        public uint MaxMp { get; set; }

        public byte Level
        {
            get => _level;
            set
            {
                _level = value;
                _nextLevelExp = GameDatabase.Get.GetNextLevelExp(value);
            }
        }
        
        public string Name { get; set; }
        public string JobName { get; set; }
        public ulong NextLevelExp => _nextLevelExp;
        public ulong JobNextLevelExp => _jobNextLevelExp;

        public byte JobLevel
        {
            get => _jobLevel;
            set
            {
                _jobLevel = value;
                _jobNextLevelExp = GameDatabase.Get.GetJobNextLevelExp(value);
            }
        }

        public ulong ExpGained { get; set; }
        public ulong JobExpGained { get; set; }
        public uint Skillpoints { get; set; }
        public ulong Gold { get; set; }

        public ushort Angle { get; set; }

        public Dictionary<string, List<InventoryItem>> Inventories = new Dictionary<string, List<InventoryItem>>();

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
            return (double) ExpGained / _nextLevelExp;
        }
        
        public double GetJobExpPercentDecimal()
        {
            return (double) JobExpGained / _jobNextLevelExp;
        }
    }
}