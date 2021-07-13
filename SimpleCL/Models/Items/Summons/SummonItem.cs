namespace SimpleCL.Models.Items.Summons
{
    public class SummonItem : InventoryItem
    {
        public Variant CosVariant => (Variant) TypeId3;
        public Pet PetType => (Pet) TypeId4;
        
        public SummonItem(uint id) : base(id)
        {
        }

        public enum Variant : byte
        {
            Summon = 1,
            Mask = 2
        }

        public enum Pet : byte
        {
            AttackPet = 1,
            PickPet = 2,
            FellowPet = 3,
            Behemoth = 4,
            Unknown
        }

        public void Summon()
        {
            
        }

        public void Terminate()
        {
            
        }
    }
}