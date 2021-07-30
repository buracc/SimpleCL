using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using SimpleCL.Database;
using SimpleCL.Enums;
using SimpleCL.Enums.Commons;
using SimpleCL.Enums.Events;
using SimpleCL.Enums.Items;
using SimpleCL.Enums.Items.Type;
using SimpleCL.Models.Exceptions;
using SimpleCL.Models.Items.Consumables;
using SimpleCL.Models.Items.Equipables;
using SimpleCL.Models.Items.JobEquipables;
using SimpleCL.Models.Items.Summons;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Items
{
    public class InventoryItem : IComparable<InventoryItem>, IDisposable
    {
        private uint _rentTypeId;
        [Browsable(false)] public uint Uid { get; set; }
        public Image Icon { get; set; }
        public byte Slot { get; set; }
        [Browsable(false)] public uint Id { get; }
        [Browsable(false)] public string ServerName { get; }
        public string Name { get; }
        [Browsable(false)] public ulong GoldValue { get; set; }

        private ushort _quantity;

        public readonly byte TypeId1;
        public readonly ItemCategory Category;
        public readonly byte TypeId3;
        public readonly byte TypeId4;
        public readonly bool CashItem;
        public readonly ushort Stack;


        public ushort Quantity
        {
            get => _quantity == 0 ? (ushort) 1 : _quantity;
            set => _quantity = value;
        }

        public InventoryItem(byte slot, uint id, string serverName, string name)
        {
            Slot = slot;
            Id = id;
            ServerName = serverName;
            Name = name;
        }

        public InventoryItem(uint id, uint rentTypeId)
        {
            var data = GameDatabase.Get.GetItemData(id);
            if (data == null)
            {
                throw new EntityParseException(id);
            }

            _rentTypeId = rentTypeId;
            Id = id;
            ServerName = data[Constants.Strings.ServerName];
            Name = data[Constants.Strings.Name];
            TypeId1 = 3;
            Category = (ItemCategory) byte.Parse(data[Constants.Strings.Tid2]);
            TypeId3 = byte.Parse(data[Constants.Strings.Tid3]);
            TypeId4 = byte.Parse(data[Constants.Strings.Tid4]);
            CashItem = byte.Parse(data[Constants.Strings.CashItem]) == 1;
            Stack = ushort.Parse(data[Constants.Strings.Stack]);
            Icon = Image.FromFile(Directory.GetCurrentDirectory() + Constants.Paths.Icons +
                                  data[Constants.Strings.Icon].Replace(Constants.Strings.Ddj, Constants.Strings.Png));
            GoldValue = ulong.Parse(data["price"]);
        }

        public static InventoryItem FromId(uint id, uint rentTypeId = 0)
        {
            var item = new InventoryItem(id, rentTypeId);
            switch (item.Category)
            {
                case ItemCategory.Equipment:
                {
                    var equip = new Equipment(id, rentTypeId);
                    if (equip.EquipmentType == Equipment.Type.Avatar)
                    {
                        return new Avatar(id, rentTypeId);
                    }

                    return equip.SlotType == Equipment.EquipmentSlot.Weapon ? new Weapon(id, rentTypeId) : equip;
                }
                case ItemCategory.Summon:
                    return new Summon(id, rentTypeId);
                case ItemCategory.Consumable:
                    var consumable = new Consumable(id, rentTypeId);
                    return consumable.Variant switch
                    {
                        ConsumableType.Variant.Scroll => new Scroll(id, rentTypeId),
                        ConsumableType.Variant.Potion => new Potion(id, rentTypeId),
                        _ => consumable
                    };

                case ItemCategory.JobEquipment:
                    return new JobEquipment(id, rentTypeId);
                case ItemCategory.FellowEquipment:
                    return new FellowEquipment(id, rentTypeId);
            }

            return item;
        }

        public int CompareTo(InventoryItem other)
        {
            return Slot - other.Slot;
        }

        public void Use()
        {
            var usePacket = new Packet(Opcode.Agent.Request.INVENTORY_ITEM_USE, true);
            usePacket.WriteByte(Slot);
            usePacket.WriteByte(CashItem ? 0x31 : 0x30);
            var usageType2 = Category switch
            {
                ItemCategory.Consumable => 0x0C,
                ItemCategory.Summon => 0x08,
                _ => 0xFF
            };

            if (usageType2 == 0xFF)
            {
                return;
            }

            usePacket.WriteByte(usageType2);
            usePacket.WriteByte(TypeId3);
            usePacket.WriteByte(TypeId4);
            usePacket.Send();
        }

        public void Move(byte currentSlot, byte targetSlot,
            InventoryAction action = InventoryAction.InventoryToInventory, ushort quantity = 0)
        {
            var packet = new Packet(Opcode.Agent.Request.INVENTORY_OPERATION);
            packet.WriteByte(action);
            packet.WriteByte(currentSlot);
            packet.WriteByte(targetSlot);

            if (action != InventoryAction.InventoryToAvatar && action != InventoryAction.AvatarToInventory)
            {
                packet.WriteUShort(quantity);
            }

            packet.Send();
        }

        public override string ToString()
        {
            return Name;
        }

        public void Dispose()
        {
            Icon?.Dispose();
        }

        ~InventoryItem()
        {
            Dispose();
        }
    }
}