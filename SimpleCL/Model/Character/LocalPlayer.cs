using System.Collections.Generic;
using SimpleCL.Database;
using SimpleCL.Model.Coord;
using SimpleCL.Model.Inventory;

namespace SimpleCL.Model.Character
{
    public class LocalPlayer
    {
        private LocalPlayer() { }
        
        private static LocalPlayer _instance;
        public static LocalPlayer Get => _instance ?? (_instance = new LocalPlayer());
        public uint Uid { get; set; }
        private ulong _nextLevelExp;
        private ulong _jobNextLevelExp;
        public uint Hp { get; set; }
        public uint Mp { get; set; }

        private byte _level;
        public byte Level
        {
            get => _level;
            set
            {
                _level = value;
                _nextLevelExp = GameDatabase.Get.GetNextLevelExp(value);
            }
        }

        public ulong NextLevelExp => _nextLevelExp;

        public ulong JobNextLevelExp => _jobNextLevelExp;

        private byte _jobLevel;
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
        public LocalPoint LocalPoint { get; set; }
        public Dictionary<string, List<InventoryItem>> Inventories = new Dictionary<string, List<InventoryItem>>();
        public string Name { get; set; }
        public string JobName { get; set; }

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