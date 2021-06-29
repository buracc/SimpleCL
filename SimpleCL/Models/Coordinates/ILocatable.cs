namespace SimpleCL.Models.Coordinates
{
    public interface ILocatable
    {
        LocalPoint LocalPoint { get; set; }
        WorldPoint WorldPoint { get; }
    }
}