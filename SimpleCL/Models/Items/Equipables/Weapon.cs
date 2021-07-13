namespace SimpleCL.Models.Items.Equipables
{
    public class Weapon : Equipment
    {
        public Type WeaponType => (Type) TypeId4;

        public Weapon(uint id) : base(id)
        {
        }

        public new enum Type : byte
        {
            Sword = 2,
            Blade = 3,
            Spear = 4,
            Glaive = 5,
            Bow = 6,
            OneHandSword = 7,
            TwoHandSword = 8,
            DualAxes = 9,
            WarlockRod = 10,
            TwoHandStaff = 11,
            Crossbow = 12,
            Daggers = 13,
            Harp = 14,
            ClericRod = 15,
            Fortress = 16
        }
    }
}