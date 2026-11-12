namespace ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;

/// <summary>
///     Represents the lifecycle status of a trip.
/// </summary>
public enum TripStatus
{
    Scheduled,
    InProgress,
    Delayed,
    Completed,
    Cancelled
}