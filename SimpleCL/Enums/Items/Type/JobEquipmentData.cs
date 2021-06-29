namespace SimpleCL.Enums.Items.Type
{
    // ItemCategory == 4
    public static class JobEquipmentData
    {
        public enum Type : byte
        {
            HunterArmor = 1,
            HunterWeapon = 2,
            HunterAccessory = 3,
            ThiefArmor = 4,
            ThiefWeapon = 5,
            ThiefAccessory = 6
        }

        public static class SubType
        {
            public enum Protector : byte
            {
                Head = 1,
                Shoulders = 2,
                Chest = 3,
                Legs = 4,
                Hands = 5,
                Feet = 6
            }
            
            public enum Accessory: byte
            {
                Earring = 1,
                Necklace = 2,
                Ring = 3
            }

            public enum Weapon : byte
            {
                All = 1
            }
        }
    }
}