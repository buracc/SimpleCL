using SimpleCL.Enums.Items.Type;

namespace SimpleCL.Models.Items.Consumables
{
    public class Scroll : Consumable
    {
        public ConsumableType.Scroll ScrollType => (ConsumableType.Scroll) TypeId4;
        
        public Scroll(uint id) : base(id)
        {
        }
    }
}