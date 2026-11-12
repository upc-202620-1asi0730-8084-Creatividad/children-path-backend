namespace ChildrenPath.Platform.Trips.Domain.Model.Queries;

/// <summary>
///     Query to get a specific trip by its unique identifier.
/// </summary>
/// /// <param name="TripId">The id of the trip to retrieve.</param>
public record GetTripByIdQuery(int TripId);