using System;

namespace SimpleCL.Model.Coord
{
    public class WorldPoint : ICoord
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public ushort Region { get; }

        public byte XSector;
        public byte YSector;

        public WorldPoint(float x, float y, float z, ushort region)
        {
            X = x;
            Y = y;
            Z = z;
            Region = region;
        }

        public WorldPoint(float x, float y, float z = 0.0f)
        {
            X = x;
            Y = y;
            Z = z;

            var xSector = (byte) Math.Round((X - x / 10) / 192.0 + 135);
            var ySector = (byte) Math.Round((Y - y / 10) / 192.0 + 92);
            var region = (ushort) ((ySector << 8) | xSector);

            XSector = xSector;
            YSector = ySector;
            Region = region;
        }

        public bool InCave()
        {
            return Region > short.MaxValue;
        }

        public WorldPoint Translate(float x = 0, float y = 0)
        {
            return new WorldPoint(X + x, Y + y);
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

            var point = new WorldPoint(
                (xSector - 135) * 192 + localPoint.X / 10,
                (ySector - 92) * 192 + localPoint.Y / 10,
                localPoint.Z,
                localPoint.Region
            );
            point.XSector = xSector;
            point.YSector = ySector;
            return point;
        }

        public override string ToString()
        {
            return "X: " + X + " | Y: " + Y;
        }
    }
}