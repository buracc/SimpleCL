using SimpleCL.Models.Items;

namespace SimpleCL.Models.Entities.Exchange
{
    public class StallItem
    {
        public byte Slot { get; set; }
        public InventoryItem RefItem { get; set; }
        public ushort Quantity { get; set; }
        public ulong Price { get; set; }

        public void Purchase()
        {
            
        }
    }
}