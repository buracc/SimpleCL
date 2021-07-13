using SimpleCL.Enums.Items.Type;

namespace SimpleCL.Models.Items.Consumables
{
    public class Consumable : InventoryItem
    {
        public ConsumableType.Variant Variant => (ConsumableType.Variant) TypeId3;
        
        public Consumable(uint id) : base(id)
        {
        }
    }
}