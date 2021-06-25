namespace SimpleCL.Model.Entity
{
    public class Teleport : Entity
    {
        public Teleport(uint id) : base(id)
        {
        }

        public bool IsNpc()
        {
            return TypeId1 == 1 && TypeId2 == 2 && TypeId3 == 2 && TypeId4 == 0;
        }

        public bool IsGate()
        {
            return TypeId1 == 4 && TypeId2 == 0 && TypeId3 == 0 && TypeId4 == 0;
        }

        public enum Type : byte
        {
            None = 0,
            Regular = 1,
            Dimensional = 6
        }
    }
}