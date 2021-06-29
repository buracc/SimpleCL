namespace SimpleCL.Enums.Events
{
    public static class EntityStateEvent
    {
        public enum Health : byte
        {
            HP = 1,
            MP = 2,
            HPMP = 3,
            BadStatus = 4,
            EntityHPMP = 5
        }
    }
}