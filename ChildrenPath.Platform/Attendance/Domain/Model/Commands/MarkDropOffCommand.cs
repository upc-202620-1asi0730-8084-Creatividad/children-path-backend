namespace ChildrenPath.Platform.Attendance.Domain.Model.Commands;

/// <summary>
///     Command to mark an existing attendance record as dropped off at the destination.
/// </summary>
/// <param name="AttendanceId">The unique identifier of the attendance record.</param>
public record MarkDropOffCommand(int AttendanceId);