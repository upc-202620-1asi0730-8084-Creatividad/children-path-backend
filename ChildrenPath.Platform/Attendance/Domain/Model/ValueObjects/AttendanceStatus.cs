namespace ChildrenPath.Platform.Attendance.Domain.Model.ValueObjects;

/// <summary>
///     Represents the lifecycle status of a student's attendance during a trip.
/// </summary>
public enum AttendanceStatus
{
    Waiting,
    Boarded,
    DroppedOff,
    Absent,
    Late,
    PendingConfirmation
}