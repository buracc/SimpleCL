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
using SimpleCL.Interaction;
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
        public Image Icon { get; set; }
        public byte Slot { get; set; }
        [Browsable(false)] public uint Id { get; }
        [Browsable(false)] public string ServerName { get; }
        public string Name { get; }

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

        public InventoryItem(uint id)
        {
            var data = GameDatabase.Get.GetItemData(id);
            if (data == null)
            {
                throw new EntityParseException(id);
            }

            Id = id;
            ServerName = data[Constants.Strings.ServerName];
            Name = data[Constants.Strings.Name];
            TypeId1 = 3;
            Category = (ItemCategory) byte.Parse(data[Constants.Strings.Tid1]);
            TypeId3 = byte.Parse(data[Constants.Strings.Tid2]);
            TypeId4 = byte.Parse(data[Constants.Strings.Tid3]);
            CashItem = byte.Parse(data[Constants.Strings.CashItem]) == 1;
            Stack = ushort.Parse(data[Constants.Strings.Stack]);
            Icon = Image.FromFile(Directory.GetCurrentDirectory() + Constants.Paths.Icons +
                                  data[Constants.Strings.Icon].Replace(Constants.Strings.Ddj, Constants.Strings.Png));
        }

        public static InventoryItem FromId(uint id)
        {
            var item = new InventoryItem(id);
            switch (item.Category)
            {
                case ItemCategory.Equipment:
                {
                    var equip = new Equipment(id);
                    if (equip.EquipmentType == Equipment.Type.Avatar)
                    {
                        return new Avatar(id);
                    }

                    return equip.SlotType == Equipment.EquipmentSlot.Weapon ? new Weapon(id) : equip;
                }
                case ItemCategory.Summon:
                    return new SummonItem(id);
                case ItemCategory.Consumable:
                    var consumable = new Consumable(id);
                    return consumable.Variant switch
                    {
                        ConsumableType.Variant.Scroll => new Scroll(id),
                        ConsumableType.Variant.Potion => new Potion(id),
                        _ => consumable
                    };

                case ItemCategory.JobEquipment:
                    return new JobEquipment(id);
                case ItemCategory.FellowEquipment:
                    return new FellowEquipment(id);
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
            usePacket.WriteByte(CashItem ? (byte) 0x31 : (byte) 0x30);
            usePacket.WriteByte(0x0C);
            usePacket.WriteByte(TypeId3);
            usePacket.WriteByte(TypeId4);
            usePacket.Send();
        }

        public void Move(byte currentSlot, byte targetSlot, InventoryAction action = InventoryAction.InventoryToInventory, ushort quantity = 0)
        {
            var packet = new Packet(Opcode.Agent.Request.INVENTORY_OPERATION);
            packet.WriteByte(action);
            packet.WriteByte(currentSlot);
            packet.WriteByte(targetSlot);
            packet.WriteUShort(quantity);
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