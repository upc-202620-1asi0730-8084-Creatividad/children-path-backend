namespace ChildrenPath.Platform.Tracking.Domain;

// --- ENUM ---
public enum TrackingStatus { Active, Paused, Stopped, Lost }

// --- VALUE OBJECTS ---
public class Location
{
    public int Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public double Speed { get; set; }
}

public class ETA
{
    public int Id { get; set; }
    public DateTime EstimatedArrival { get; set; }
    public int MinutesRemaining { get; set; }
    public int StopId { get; set; } // Referencia al contexto de Rutas
}

// --- ENTIDAD PRINCIPAL (Aggregate Root) ---
public class TrackingSession
{
    public int Id { get; set; }
    
    // Referencias a otros contextos
    public int TripId { get; set; }
    public int VehicleId { get; set; }
    
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; }
    public TrackingStatus Status { get; set; } = TrackingStatus.Active;

    // Composición (según tu UML)
    public List<Location> Locations { get; set; } = new();
    public List<ETA> ETAs { get; set; } = new();

    // Métodos del UML
    public void Start()
    {
        Status = TrackingStatus.Active;
        StartedAt = DateTime.UtcNow;
    }

    public void Stop()
    {
        Status = TrackingStatus.Stopped;
        EndedAt = DateTime.UtcNow;
    }
}