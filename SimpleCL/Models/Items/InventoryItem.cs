using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using SimpleCL.Database;
using SimpleCL.Enums;
using SimpleCL.Enums.Commons;
using SimpleCL.Interaction;
using SimpleCL.Models.Exceptions;
using SimpleCL.SecurityApi;

namespace SimpleCL.Models.Items
{
    /// <summary>
    /// todo:
    /// - make items disposable
    /// - dispose images before disposing item
    /// - same thing to buffs
    /// - correctly clear all items/buffs on entity despawn
    /// </summary>
    public class InventoryItem : IComparable<InventoryItem>, IDisposable
    {
        public Image Icon { get; set; }
        public byte Slot { get; set; }
        [Browsable(false)]
        public uint Id { get; }
        [Browsable(false)]
        public string ServerName { get; }
        public string Name { get; }

        private ushort _quantity;

        public readonly byte TypeId1;
        public readonly byte TypeId2;
        public readonly byte TypeId3;
        public readonly byte TypeId4;
        public readonly bool CashItem;

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
            TypeId2 = byte.Parse(data[Constants.Strings.Tid1]);
            TypeId3 = byte.Parse(data[Constants.Strings.Tid2]);
            TypeId4 = byte.Parse(data[Constants.Strings.Tid3]);
            CashItem = byte.Parse(data[Constants.Strings.CashItem]) == 1;
            Icon = Image.FromFile(Directory.GetCurrentDirectory() + Constants.Paths.Icons + data[Constants.Strings.Icon].Replace(Constants.Strings.Ddj, Constants.Strings.Png));
        }

        public static InventoryItem FromId(uint id)
        {
            return new(id);
        }

        public bool IsEquipment()
        {
            return TypeId2 == 1;
        }

        public bool IsCos()
        {
            return TypeId2 == 2;
        }

        public bool IsConsumable()
        {
            return TypeId2 == 3;
        }

        public int CompareTo(InventoryItem other)
        {
            return Slot - other.Slot;
        }

        public void Use()
        {
            var usePacket = new Packet(Opcodes.Agent.Request.INVENTORY_ITEM_USE, true);
            usePacket.WriteByte(Slot);
            usePacket.WriteByte(CashItem ? (byte) 0x31 : (byte) 0x30);
            usePacket.WriteByte(0x0C);
            usePacket.WriteByte(TypeId3);
            usePacket.WriteByte(TypeId4);
            InteractionQueue.PacketQueue.Enqueue(usePacket);
        }

        public override string ToString()
        {
            return Name;
        }

        public void Dispose()
        {
            Icon?.Dispose();
        }
    }
}