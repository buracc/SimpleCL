namespace SimpleCL.Model.Coord
{
    public class LocalPoint
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public ushort Region { get; set; }
        public ushort Angle { get; set; }

        public LocalPoint(ushort region, float x, float z, float y, ushort angle)
        {
            X = x;
            Y = y;
            Z = z;
            Region = region;
            Angle = angle;
        }

        public override string ToString()
        {
            return "X: " + (int) X + " | Y: " + (int) Y + " | Z: " + (int) Z + " | Region: " + Region;
        }
    }
}