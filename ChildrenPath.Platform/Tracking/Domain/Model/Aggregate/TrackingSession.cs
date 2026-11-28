using ChildrenPath.Platform.Shared.Domain.Model.Aggregates;
using ChildrenPath.Platform.Tracking.Domain.Model.Entities;
using ChildrenPath.Platform.Tracking.Domain.Model.Errors;
using ChildrenPath.Platform.Tracking.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Tracking.Domain.Model.Aggregate;

public enum TrackingStatus { Active, Paused, Stopped, Lost }

public partial class TrackingSession : AggregateRoot
{
    private readonly List<Location> _locations = new();
    private readonly List<ETA> _etas = new();

    public IReadOnlyCollection<Location> Locations => _locations.AsReadOnly();
    public IReadOnlyCollection<ETA> ETAs => _etas.AsReadOnly();

    public TrackingSessionId Id { get; private set; }
    public int TripId { get; private set; }
    public int VehicleId { get; private set; }
    
    public DateTime StartedAt { get; private set; }
    public DateTime? EndedAt { get; private set; }
    public TrackingStatus Status { get; private set; }

    public TrackingSession() { }

    public TrackingSession(int tripId, int vehicleId, double initialLat, double initialLng, double speed)
    {
        TripId = tripId;
        VehicleId = vehicleId;
        StartedAt = DateTime.UtcNow;
        Status = TrackingStatus.Active;
        
        AddLocation(initialLat, initialLng, speed);
    }

    // --- Métodos de Dominio ---
    
    public void AddLocation(double latitude, double longitude, double speed)
    {
        if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            throw new ArgumentException(TrackingErrors.InvalidCoordinates.Message);

        _locations.Add(new Location(latitude, longitude, speed));
    }

    public void AddETA(DateTime estimatedArrival, int minutesRemaining, int stopId)
    {
        _etas.Add(new ETA(estimatedArrival, minutesRemaining, stopId));
    }

    public void StopTracking()
    {
        if (Status == TrackingStatus.Stopped) return;
        
        Status = TrackingStatus.Stopped;
        EndedAt = DateTime.UtcNow;
    }
}