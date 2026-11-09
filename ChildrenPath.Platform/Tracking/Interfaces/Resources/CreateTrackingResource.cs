namespace ChildrenPath.Platform.Tracking.Interfaces.Resources;

public record CreateTrackingResource(
    int TripId,
    int VehicleId,
    double InitialLatitude,
    double InitialLongitude,
    double Speed
);