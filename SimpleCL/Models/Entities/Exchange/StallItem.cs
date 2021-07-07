using SimpleCL.Enums.Commons;
using SimpleCL.Interaction;
using SimpleCL.Models.Items;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Entities.Exchange
{
    public class StallItem
    {
        public byte Slot { get; set; }
        public InventoryItem Item { get; set; }
        public ushort Quantity { get; set; }
        public ulong Price { get; set; }

        public void Purchase()
        {
            Program.Gui.Log("Purchasing: " + Item.Name + " for " + Price);
            var buyPacket = new Packet(Opcodes.Agent.Request.STALL_BUY);
            buyPacket.WriteByte(Slot);
            InteractionQueue.PacketQueue.Enqueue(buyPacket);
        }

        public override string ToString()
        {
            return Quantity + "x " + Item.Name + " [" + Price.ToString("N") + "]";
        }
    }
}