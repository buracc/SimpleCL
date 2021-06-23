using System.Collections.Generic;
using System.Collections.Specialized;
using SimpleCL.Database;
using SimpleCL.Model.Coord;
using SimpleCL.Model.Inventory;

namespace SimpleCL.Model.Character
{
    public class Character
    {
        private ulong _nextLevelExp;
        
        public uint Hp { get; }
        public uint Mp { get; }

        private byte _level;
        public byte Level
        {
            get => _level;
            set
            {
                _level = value;
                _nextLevelExp = GameDatabase.GetInstance().GetNextLevelExp(Level);
            }
        }

        public ulong ExpGained { get; }
        public uint Skillpoints { get; }
        public ulong Gold { get;  }
        public Coordinates Coordinates { get; }
        public Dictionary<string, List<Item>> Inventories = new Dictionary<string, List<Item>>();

        public Character(uint hp, uint mp, byte level, ulong expGained, uint skillpoints, ulong gold, Coordinates coordinates)
        {
            Hp = hp;
            Mp = mp;
            Level = level;
            ExpGained = expGained;
            Skillpoints = skillpoints;
            Gold = gold;
            Coordinates = coordinates;
        }

        public uint getHpPercent()
        {
            return 100;
        }
        
        public uint getMpPercent()
        {
            return 100;
        }

        public int GetExpPercent()
        {
            return (int) (100.0 / _nextLevelExp * ExpGained);
        }
    }
}