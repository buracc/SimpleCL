using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using SimpleCL.Database;
using SimpleCL.Enums;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Events;
using SimpleCL.Interaction;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Items
{
    public class ShopItem : InventoryItem
    {
        public uint Price { get; }
        public readonly byte Tab;
        [Browsable(false)]
        public uint ShopUid { get; set; }

        public ShopItem(uint id, NameValueCollection shopData) : base(id)
        {
            var nvc = GameDatabase.Get.GetItemPrice(id);
            if (nvc == null)
            {
                Console.WriteLine("Shopitem " + id + " not found");
                return;
            }

            Quantity = 1;
            Price = uint.Parse(nvc[Constants.Strings.Price]);
            Tab = byte.Parse(shopData[Constants.Strings.Tab]);
            Slot = byte.Parse(shopData[Constants.Strings.Slot]);
        }

        public void Purchase(int amount)
        {
            var buyPacket = new Packet(Opcode.Agent.Request.INVENTORY_OPERATION);
            buyPacket.WriteByte((byte) InventoryAction.ShopToInventory);
            buyPacket.WriteByte(Tab);
            buyPacket.WriteByte(Slot);
            buyPacket.WriteUShort((ushort) amount);
            buyPacket.WriteUInt(ShopUid);
            buyPacket.Send();
        }
    }
}