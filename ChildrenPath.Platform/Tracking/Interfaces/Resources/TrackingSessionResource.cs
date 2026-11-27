namespace ChildrenPath.Platform.Tracking.Interfaces.Resources;

public record TrackingSessionResource(
    int Id,
    int TripId,
    int VehicleId,
    string Status,
    DateTime StartedAt,
    double CurrentLatitude,
    double CurrentLongitude,
    double CurrentSpeed
);