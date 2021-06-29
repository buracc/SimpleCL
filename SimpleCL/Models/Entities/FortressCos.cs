namespace SimpleCL.Models.Entities
{
    public class FortressCos : Npc
    {
        public FortressCos(uint id) : base(id)
        {
        }

        public bool IsPatrolGuard()
        {
            return TypeId4 == 1;
        }

        public bool IsFortressFlag()
        {
            return TypeId4 == 2;
        }

        public bool IsFortressMonster()
        {
            return TypeId4 == 3;
        }

        public bool IsDefenseGuard()
        {
            return TypeId4 == 4;
        }
    }
}