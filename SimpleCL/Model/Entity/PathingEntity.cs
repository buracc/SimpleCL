namespace SimpleCL.Model.Entity
{
    public class PathingEntity : Entity
    {
        public PathingEntity(uint id) : base(id)
        {
            
        }

        public bool IsPlayer()
        {
            return TypeId2 == 1;
        }

        public bool IsNpc()
        {
            return TypeId2 == 2;
        }
    }
}