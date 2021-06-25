namespace SimpleCL.Model.Entity
{
    public class Npc : PathingEntity
    {
        public Npc(uint id) : base(id)
        {
        }

        public bool IsMonster()
        {
            return TypeId3 == 1;
        }

        public bool IsGuide()
        {
            return TypeId3 == 2;
        }

        public bool IsCos()
        {
            return TypeId3 == 3;
        }

        public bool IsFortressCos()
        {
            return TypeId3 == 4;
        }

        public bool IsFortressStructure()
        {
            return TypeId3 == 5;
        }

        /// <summary>
        /// Battlefield of Infinity
        /// </summary>
        /// <returns></returns>
        public bool IsGuardianStone()
        {
            return TypeId3 == 6;
        }

        public bool IsBoiRewardBox()
        {
            return TypeId3 == 7;
        }

        public bool IsSurvivalSafezone()
        {
            return TypeId3 == 8;
        }
    }
}