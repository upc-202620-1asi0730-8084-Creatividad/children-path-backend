namespace ChildrenPath.Platform.Alerts.Domain.Model.Queries;

/// <summary>
///     Query to get all alerts belonging to a specific trip.
/// </summary>
/// <param name="TripId">The id of the trip to get alerts for.</param>
public record GetAlertsByTripIdQuery(int TripId);