using System;

namespace SimpleCL.Model.Inventory
{
    public class Item : IComparable<Item>
    {
        public byte Slot { get;  }
        public uint Id { get; }
        public string ServerName { get; }
        public string Name { get; }

        private ushort _quantity;

        public ushort Quantity
        {
            get => _quantity == 0 ? (ushort) 1 : _quantity;
            set => _quantity = value;
        }

        public Item(byte slot, uint id, string serverName, string name)
        {
            Slot = slot;
            Id = id;
            ServerName = serverName;
            Name = name;
        }

        public int CompareTo(Item other)
        {
            return Slot - other.Slot;
        }
    }
}