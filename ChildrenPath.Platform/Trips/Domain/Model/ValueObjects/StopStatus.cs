namespace ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;

/// <summary>
///     Represents the status of a route stop during a trip.
/// </summary>
public enum StopStatus
{
    Pending,
    Reached,
    Skipped
}