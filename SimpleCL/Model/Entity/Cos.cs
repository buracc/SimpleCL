namespace SimpleCL.Model.Entity
{
    public class Cos : Npc
    {
        public Cos(uint id) : base(id)
        {
        }

        public bool IsHorse()
        {
            return TypeId4 == 1;
        }

        public bool IsTransport()
        {
            return TypeId4 == 2;
        }

        public bool IsAttackPet()
        {
            return TypeId4 == 3;
        }

        public bool IsPickPet()
        {
            return TypeId4 == 4;
        }

        public bool IsGuildGuard()
        {
            return TypeId4 == 5;
        }

        public bool IsQuestPet()
        {
            return TypeId4 == 6;
        }

        public bool IsThiefCaravan()
        {
            return TypeId4 == 7;
        }

        public bool IsFlower()
        {
            return TypeId4 == 8;
        }

        public bool IsFellowPet()
        {
            return TypeId4 == 9;
        }
    }
}