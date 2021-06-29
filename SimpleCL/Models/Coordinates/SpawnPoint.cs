namespace SimpleCL.Models.Coordinates
{
    public class SpawnPoint : ILocatable, IIdentifiable
    {
        public readonly uint MonsterId;
        public readonly string MonsterName;
        public LocalPoint LocalPoint { get; set; }
        public WorldPoint WorldPoint => WorldPoint.FromLocal(LocalPoint);
        private uint _uid;

        public uint Uid
        {
            get =>
                (uint) (MonsterId + LocalPoint.X + LocalPoint.Y +
                        LocalPoint.Z + LocalPoint.Region);
            set => throw new System.NotImplementedException();
        }

        public SpawnPoint(LocalPoint localPoint, uint monsterId, string monsterName)
        {
            LocalPoint = localPoint;
            MonsterId = monsterId;
            MonsterName = monsterName;
        }

        public override string ToString()
        {
            return MonsterName + " [" + WorldPoint + "]";
        }
    }
}