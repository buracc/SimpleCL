using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Text;
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
        public string Pricing { get; }


        public ShopItem(uint id, NameValueCollection shopData, uint rentTypeId) : base(id, rentTypeId)
        {
            Quantity = 1;
            Tab = byte.Parse(shopData[Constants.Strings.Tab]);
            Slot = byte.Parse(shopData[Constants.Strings.Slot]);
            Pricing = ParsePrices(shopData["prices"]);
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

        public string ParsePrices(string priceString)
        {
            var sb = new StringBuilder();
            var prices = priceString.Split(',');
            foreach (var price in prices)
            {
                var split = price.Split('=');
                var currency = uint.Parse(split[0]) switch
                {
                    1 => "Gold",
                    2 => "Silk",
                    4 => "Gift Silk",
                    16 => "Premium Silk",
                    32 => "Honor Point",
                    64 => "Copper Coin",
                    128 => "Iron Coin",
                    256 => "Silver Coin",
                    512 => "Gold Coin",
                    1024 => "Arena Coin",
                    8192 => "Monk's Token",
                    16384 => "Soldier's Token",
                    32768 => "General's Token",
                    65536 => "Coin of Combativeness (Party)",
                    131072 => "Coin of Combativeness (Individual)",
                    _ => split[0]
                };
                var value = uint.Parse(split[1]);

                sb.Append($"{currency}: {value:N}  ");
            }
            
            return sb.ToString();
        }
    }
}