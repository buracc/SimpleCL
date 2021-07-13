using SimpleCL.Enums.Items.Type;

namespace SimpleCL.Models.Items.Consumables
{
    public class Potion : Consumable
    {
        public ConsumableType.Potion PotionType => (ConsumableType.Potion) TypeId4;
        
        public Potion(uint id) : base(id)
        {
        }

        public new void Use()
        {
            
        }
    }
}