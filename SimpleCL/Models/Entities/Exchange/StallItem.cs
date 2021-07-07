using System;
using System.ComponentModel;
using SimpleCL.Models.Items;

namespace SimpleCL.Models.Entities.Exchange
{
    public class StallItem
    {
        [Browsable(false)]
        public byte Slot { get; set; }
        public InventoryItem Item { get; set; }
        public ushort Quantity { get; set; }
        public ulong Price { get; set; }

        public void Purchase()
        {
            Program.Gui.Log("Purchasing: " + Item.Name + " for " + Price);
        }

        public override string ToString()
        {
            return Quantity + "x " + Item.Name + " [" + Price.ToString("N") + "]";
        }
    }
}