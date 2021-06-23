namespace SimpleCL.Model
{
    public class Coordinates
    {
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public ushort Region { get; }
        public ushort Angle { get; }

        public Coordinates(float x, float y, float z, ushort region, ushort angle)
        {
            X = x;
            Y = y;
            Z = z;
            Region = region;
            Angle = angle;
        }

        public override string ToString()
        {
            return X + ", " + Y;
        }
    }
}