namespace SimpleCL.Enums.Items.Type
{
    // ItemCategory == 2
    public static class CosType
    {
        public enum Variant : byte
        {
            Summon = 1,
            Mask = 2
        }

        public enum Summon : byte
        {
            AttackPet = 1,
            GrabPet = 2,
            Fellow = 3,
            Trade = 4
        }

        public enum Mask : byte
        {
            Mask = 0
        }
    }
}