namespace SimpleCL.Models.Entities
{
    public class FortressStructure : Npc
    {
        public FortressStructure(uint id) : base(id)
        {
        }

        public bool IsHeart()
        {
            return TypeId4 == 1;
        }

        public bool IsTower()
        {
            return TypeId4 == 2;
        }

        public bool IsGate()
        {
            return TypeId4 == 3;
        }

        public bool IsDefenseCamp()
        {
            return TypeId4 == 4;
        }

        public bool IsCommandPost()
        {
            return TypeId4 == 5;
        }

        public bool IsObstacle()
        {
            return TypeId4 == 6;
        }
    }
}