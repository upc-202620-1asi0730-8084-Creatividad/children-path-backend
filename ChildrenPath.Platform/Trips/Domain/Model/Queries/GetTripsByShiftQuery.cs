using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Trips.Domain.Model.Queries;

/// <summary>
///     Query to get all trips filtered by operational shift.
/// </summary>
/// <param name="Shift">The shift (Morning/Afternoon) to filter by.</param>


public record GetTripsByShiftQuery(TripShift Shift);