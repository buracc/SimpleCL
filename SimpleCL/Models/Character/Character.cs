using System;
using System.Text;

namespace SimpleCL.Models.Character
{
    public class Character
    {
        public string Name { get; }
        public byte Level { get; }
        public uint Hp { get; set; }
        public uint Mp { get; set; }
        public bool Deleting { get; }
        public DateTime DeletionTime { get; set; }

        public Character(string name, byte level, bool deleting)
        {
            Name = name;
            Level = level;
            Deleting = deleting;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(Name + " Lvl. " + Level);
            if (!Deleting)
            {
                return sb.ToString();
            }
            
            sb.Append(" Deleted at: ");
            sb.Append(DeletionTime);

            return sb.ToString();
        }
    }
}