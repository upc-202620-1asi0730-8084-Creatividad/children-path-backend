namespace ChildrenPath.Platform.Attendance.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a specific attendance record by its identifier.
/// </summary>
/// <param name="AttendanceId">The unique identifier of the attendance record.</param>
public record GetAttendanceByIdQuery(int AttendanceId);