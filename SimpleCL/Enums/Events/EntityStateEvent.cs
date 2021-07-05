namespace SimpleCL.Enums.Events
{
    public static class EntityStateEvent
    {
        public enum Health : byte
        {
            Hp = 1,
            Mp = 2,
            HpMp = 3,
            BadStatus = 4,
            EntityHpMp = 5
        }
    }
}