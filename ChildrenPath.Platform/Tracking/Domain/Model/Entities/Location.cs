namespace ChildrenPath.Platform.Tracking.Domain.Model.Entities;

public class Location
{
    public int Id { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTime Timestamp { get; private set; }
    public double Speed { get; private set; }

    // Constructor vacío requerido por Entity Framework
    public Location() { }

    // Constructor que pide tu Agregado
    public Location(double latitude, double longitude, double speed)
    {
        Latitude = latitude;
        Longitude = longitude;
        Speed = speed;
        Timestamp = DateTime.UtcNow;
    }
}