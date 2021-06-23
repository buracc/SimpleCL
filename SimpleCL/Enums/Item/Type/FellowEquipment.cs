namespace SimpleCL.Enums.Item.Type
{
    // ItemCategory == 5
    public static class FellowEquipment
    {
        public enum Type : byte
        {
            Claw = 1,
            Charm = 2,
            Saddle = 3,
            Scale = 4,
            Amulet = 5,
            Choker = 6,
            Tattoo = 7
        }

        public static class SubType
        {
            public enum All : byte
            {
                All = 0
            }
        }
    }
}