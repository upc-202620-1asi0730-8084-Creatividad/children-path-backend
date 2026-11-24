namespace ChildrenPath.Platform.Attendance.Domain.Model.Queries;

/// <summary>
///     Query to retrieve all attendance records belonging to a specific student.
/// </summary>
/// <param name="StudentId">The unique identifier of the student.</param>
public record GetAttendancesByStudentIdQuery(int StudentId);