namespace ChildrenPath.Platform.Trips.Domain.Model.Commands;

/// <summary>
///     Command to mark an active trip as completed.
/// </summary>
/// <param name="TripId">The ID of the trip to complete.</param>
public record CompleteTripCommand(int TripId);