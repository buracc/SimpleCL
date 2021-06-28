namespace SimpleCL.Model.Coord
{
    public interface ILocatable
    {
        LocalPoint LocalPoint { get; set; }
        WorldPoint WorldPoint { get; }
    }
}