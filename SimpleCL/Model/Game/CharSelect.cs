using System;
using System.Text;

namespace SimpleCL.Model.Game
{
    public class CharSelect
    {
        public string Name { get; }
        private byte CurLevel;
        private bool Deleting;
        public DateTime DeletionTime { get; set; }

        public CharSelect(string name, byte curLevel, bool deleting)
        {
            Name = name;
            CurLevel = curLevel;
            Deleting = deleting;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Name + " Lvl. " + CurLevel);
            if (Deleting)
            {
                sb.Append(" Deleted at: ");
                sb.Append(DeletionTime);
            }
            
            return sb.ToString();
        }
    }
}