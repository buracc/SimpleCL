using System.Collections.Generic;
using SimpleCL.Models.Items;

namespace SimpleCL.Models.Entities
{
    public class Player : PathingEntity
    {
        public List<InventoryItem> InventoryItems = new List<InventoryItem>();
        public Player(uint id) : base(id)
        {
        }

        public bool IsWearingJobSuit()
        {
            return InventoryItems.Exists(item => item.IsEquipment() && item.TypeId3 == 7);
        }
    }
}