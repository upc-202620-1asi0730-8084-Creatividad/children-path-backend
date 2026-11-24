using ChildrenPath.Platform.Attendance.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Attendance.Domain.Model.Commands;

/// <summary>
///     Command to update the status of an existing attendance record.
/// </summary>
/// <param name="AttendanceId">The unique identifier of the attendance record to update.</param>
/// <param name="Status">The new attendance status to apply.</param>
/// <param name="Location">The optional boarding location when transitioning to boarded.</param>
/// <param name="AbsenceReason">The optional absence reason when transitioning to absent.</param>
/// <param name="IsAbsenceJustified">Indicates whether the absence is justified.</param>
public record UpdateAttendanceStatusCommand(
    int AttendanceId,
    AttendanceStatus Status,
    string? Location = null,
    string? AbsenceReason = null,
    bool IsAbsenceJustified = false);