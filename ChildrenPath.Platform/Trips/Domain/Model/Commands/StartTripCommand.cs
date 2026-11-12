namespace ChildrenPath.Platform.Trips.Domain.Model.Commands;

/// <summary>
///     Command to start a scheduled trip.
/// </summary>
/// <param name="TripId">The ID of the trip to start.</param>
public record StartTripCommand(int TripId);