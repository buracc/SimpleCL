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
        public readonly byte Tab;
        [Browsable(false)]
        public uint ShopUid { get; set; }

        public ShopItem(uint id, NameValueCollection shopData, uint rentTypeId) : base(id, rentTypeId)
        {
            Quantity = 1;
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