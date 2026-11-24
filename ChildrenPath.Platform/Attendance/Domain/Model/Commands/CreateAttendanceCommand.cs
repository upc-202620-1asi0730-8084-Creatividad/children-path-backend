namespace ChildrenPath.Platform.Attendance.Domain.Model.Commands;

/// <summary>
///     Command to register a new attendance entry for a student on a trip.
/// </summary>
/// <param name="StudentId">The unique identifier of the student.</param>
/// <param name="TripId">The unique identifier of the trip.</param>
/// <param name="Location">The optional pickup location if the student is already boarding.</param>
/// <param name="AbsenceReason">The optional reason if the student is initially marked absent.</param>
/// <param name="IsAbsenceJustified">Indicates whether the reported absence is justified.</param>
public record CreateAttendanceCommand(
    int StudentId,
    int TripId,
    string? Location,
    string? AbsenceReason,
    bool IsAbsenceJustified);