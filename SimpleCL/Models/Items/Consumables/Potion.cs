using SimpleCL.Enums.Items.Type;

namespace SimpleCL.Models.Items.Consumables
{
    public class Potion : Consumable
    {
        public ConsumableType.Potion PotionType => (ConsumableType.Potion) TypeId4;
        
        public Potion(uint id, uint rentTypeId) : base(id, rentTypeId)
        {
        }

        public new void Use()
        {
            
        }
    }
}