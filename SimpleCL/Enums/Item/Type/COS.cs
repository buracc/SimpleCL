namespace SimpleCL.Enums.Item.Type
{
    // ItemCategory == 2
    public static class COS
    {
        public enum Type : byte
        {
            Summon = 1,
            Mask = 2
        }

        public static class SubType
        {
            public enum Summon : byte
            {
                AttackPet = 1,
                GrabPet = 2,
                Fellow = 3,
                Trade = 4
            }

            public enum Mask : byte
            {
                All = 0
            }
        }
    }
}