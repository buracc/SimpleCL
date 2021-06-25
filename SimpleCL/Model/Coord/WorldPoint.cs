namespace SimpleCL.Model.Coord
{
    public class WorldPoint : ICoord
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public ushort Region { get; }

        public WorldPoint(float x, float y, float z, ushort region)
        {
            X = x;
            Y = y;
            Z = z;
            Region = region;
        }
        
        public WorldPoint(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        public WorldPoint(float x, float y)
        {
            X = x;
            Y = y;
            Z = 0.0f;
        }

        public bool InCave()
        {
            return Region > short.MaxValue;
        }

        public double DistanceTo(float x, float y)
        {
            throw new System.NotImplementedException();
        }
        
        public double DistanceTo(WorldPoint other)
        {
            return DistanceTo(other.X, other.Y);
        }
        
        public static WorldPoint FromLocal(LocalPoint localPoint)
        {
            if (localPoint.InCave())
            {
                return new WorldPoint(localPoint.X, localPoint.Y, localPoint.Z, localPoint.Region);
            }

            var xSector = (byte) (localPoint.Region & 0xFF);
            var ySector = (byte) (localPoint.Region >> 8);
            
            return new WorldPoint((int) ((xSector - 135) * 192 + localPoint.X / 10), (int) ((ySector - 92) * 192 + localPoint.Y / 10), (int) localPoint.Z, localPoint.Region); 
        }

        public override string ToString()
        {
            return "X: " + X + " | Y: " + Y + " | Z: " + Z + " | Region: " + Region;
        }
    }
}