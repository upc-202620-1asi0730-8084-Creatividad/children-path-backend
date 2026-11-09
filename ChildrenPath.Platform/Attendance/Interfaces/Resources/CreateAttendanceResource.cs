namespace ChildrenPath.Platform.Attendance.Interfaces.Resources;

public record CreateAttendanceResource(
    int StudentId,
    int TripId,
    string Location,
    string? AbsenceReason,
    bool IsAbsenceJustified
);