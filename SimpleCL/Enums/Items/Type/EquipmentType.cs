namespace SimpleCL.Enums.Items.Type
{
    // ItemCategory == 1
    public static class EquipmentType
    {
        public enum Slot : byte
        {
            Garment = 1,
            Protector = 2,
            Armor = 3,
            Shield = 4,
            ChineseAccessory = 5,
            Weapon = 6,
            Job = 7,
            Robe = 9,
            LightArmor = 10,
            HeavyArmor = 11,
            EuropeanAccessory = 12,
            Avatar = 13,
            DevilSpirit = 14
        }

        public enum Protector : byte
        {
            Head = 1,
            Shoulders = 2,
            Chest = 3,
            Legs = 4,
            Hands = 5,
            Feet = 6
        }

        public enum Shield : byte
        {
            Chinese = 1,
            European = 2
        }

        public enum Accessory : byte
        {
            Earring = 1,
            Necklace = 2,
            Ring = 3
        }

        public enum Job : byte
        {
            TraderBag = 1,
            ThiefSuit = 2,
            HunterCard = 3,
            PvpCape = 5,
            TraderSuit = 6,
            NewHunterSuit = 7
        }

        public enum Avatar : byte
        {
            Hat = 1,
            Dress = 2,
            Attachment = 3,
            Flag = 4
        }

        public enum Weapon : byte
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
            ClericRod = 15
        }

        public enum DevilSpirit : byte
        {
            DevilSpirit = 1
        }
    }
}