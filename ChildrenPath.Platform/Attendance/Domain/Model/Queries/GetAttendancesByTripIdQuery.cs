namespace ChildrenPath.Platform.Attendance.Domain.Model.Queries;

/// <summary>
///     Query to retrieve all attendance records belonging to a specific trip.
/// </summary>
/// <param name="TripId">The unique identifier of the trip.</param>
public record GetAttendancesByTripIdQuery(int TripId);