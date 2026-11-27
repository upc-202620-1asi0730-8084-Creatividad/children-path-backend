namespace ChildrenPath.Platform.Tracking.Domain.Model.Commands;

public record CreateTrackingSessionCommand(
    int TripId, 
    int VehicleId, 
    double InitialLatitude, 
    double InitialLongitude, 
    double Speed
);