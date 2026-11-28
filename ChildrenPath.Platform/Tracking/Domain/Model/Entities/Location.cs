namespace ChildrenPath.Platform.Tracking.Domain.Model.Entities;

public class Location
{
    public int Id { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTime Timestamp { get; private set; }
    public double Speed { get; private set; }

    public Location() { }

    public Location(double latitude, double longitude, double speed)
    {
        Latitude = latitude;
        Longitude = longitude;
        Speed = speed;
        Timestamp = DateTime.UtcNow;
    }
}