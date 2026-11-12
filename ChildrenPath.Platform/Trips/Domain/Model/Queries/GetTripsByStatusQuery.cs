using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Trips.Domain.Model.Queries;

/// <summary>
///     Query to get all trips filtered by lifecycle status.
/// </summary>
/// <param name="Status">The status to filter by.</param>
public record GetTripsByStatusQuery(TripStatus Status);