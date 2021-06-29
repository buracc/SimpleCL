using System;
using SimpleCL.Database;

namespace SimpleCL.Models.Items
{
    public class InventoryItem : IComparable<InventoryItem>
    {
        public byte Slot { get;  }
        public uint Id { get; }
        public string ServerName { get; }
        public string Name { get; }

        private ushort _quantity;

        public readonly byte TypeId1;
        public readonly byte TypeId2;
        public readonly byte TypeId3;
        public readonly byte TypeId4;

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
                // Console.WriteLine("Couldn't parse inventory item with id " + id);
                return;
            }

            Id = id;
            ServerName = data["servername"];
            Name = data["name"];
            TypeId1 = 3;
            TypeId2 = byte.Parse(data["tid1"]);
            TypeId3 = byte.Parse(data["tid2"]);
            TypeId4 = byte.Parse(data["tid3"]);
        }

        public static InventoryItem FromId(uint id)
        {
            return new InventoryItem(id);
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
    }
}