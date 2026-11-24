namespace ChildrenPath.Platform.Attendance.Interfaces.Resources;

/// <summary>
///     Request payload for creating a new attendance record.
/// </summary>
/// <param name="StudentId">The identifier of the student.</param>
/// <param name="TripId">The identifier of the trip.</param>
/// <param name="Location">The optional boarding location.</param>
/// <param name="AbsenceReason">The optional absence reason if creating an absent record.</param>
/// <param name="IsAbsenceJustified">Indicates whether the absence is justified.</param>
public record CreateAttendanceResource(
    int StudentId,
    int TripId,
    string? Location = null,
    string? AbsenceReason = null,
    bool IsAbsenceJustified = false);