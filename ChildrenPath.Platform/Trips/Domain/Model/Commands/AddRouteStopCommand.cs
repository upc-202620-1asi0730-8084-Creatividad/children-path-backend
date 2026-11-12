namespace ChildrenPath.Platform.Trips.Domain.Model.Commands;

/// <summary>
///     Command to append a scheduled stop to a trip route.
/// </summary>
public record AddRouteStopCommand(
    int TripId,
    string Name,
    string Address,
    int Order,
    string ScheduledTime);