using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using SimpleCL.Database;
using SimpleCL.Enums;
using SimpleCL.Enums.Commons;
using SimpleCL.Interaction;
using SimpleCL.Models.Items;
using SimpleCL.SecurityApi;
using SimpleCL.Services.Game;

namespace SimpleCL.Models.Entities
{
    public class Shop : TalkNpc
    {
        public readonly BindingList<ShopItem> Items = new();

        public Shop(uint id, IEnumerable<NameValueCollection> shopData) : base(id)
        {
            foreach (var nvc in shopData)
            {
                Items.Add(new ShopItem(uint.Parse(nvc[Constants.Strings.Item]), nvc));
            }
        }

        public void Select()
        {
            var shopPacket = new Packet(Opcode.Agent.Request.ENTITY_SELECT_OBJECT);
            shopPacket.WriteUInt(Uid);

            foreach (var shopItem in Items)
            {
                shopItem.ShopUid = Uid;
            }

            NpcService.SelectedShop = this;
            shopPacket.Send();
        }

        public void Open()
        {
            var shopPacket = new Packet(Opcode.Agent.Request.ENTITY_NPC_OPEN);
            shopPacket.WriteUInt(Uid);
            shopPacket.WriteByte(1);
            shopPacket.Send();
        }

        public void Close()
        {
            var closePacket = new Packet(Opcode.Agent.Request.ENTITY_NPC_CLOSE);
            closePacket.WriteUInt(Uid);
            closePacket.Send();
        }
    }
}