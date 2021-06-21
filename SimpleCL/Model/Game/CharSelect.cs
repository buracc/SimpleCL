using System;
using System.Text;

namespace SimpleCL.Model.Game
{
    public class CharSelect
    {
        public string Name { get; }
        public byte Level { get; }
        public bool Deleting { get; }
        public DateTime DeletionTime { get; set; }

        public CharSelect(string name, byte level, bool deleting)
        {
            Name = name;
            Level = level;
            Deleting = deleting;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Name + " Lvl. " + Level);
            if (Deleting)
            {
                sb.Append(" Deleted at: ");
                sb.Append(DeletionTime);
            }
            
            return sb.ToString();
        }
    }
}