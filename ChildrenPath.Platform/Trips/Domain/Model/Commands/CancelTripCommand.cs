namespace ChildrenPath.Platform.Trips.Domain.Model.Commands;

/// <summary>
///     Command to cancel an existing trip.
/// </summary>
/// <param name="TripId">The ID of the trip to cancel.</param>
public record CancelTripCommand(int TripId);