namespace SimpleCL.Models.Items.Equipables
{
    public class FellowEquipment : InventoryItem
    {
        public EquipmentSlot SlotType => (EquipmentSlot) TypeId3;
        public FellowEquipment(uint id) : base(id)
        {
        }

        public enum EquipmentSlot : byte
        {
            Claw = 1,
            Charm = 2,
            Saddle = 3,
            Scale = 4,
            Amulet = 5,
            Choker = 6,
            Tattoo = 7
        }
    }
}