using ChildrenPath.Platform.Attendance.Domain.Model.Entities;
using ChildrenPath.Platform.Attendance.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Attendance.Domain.Model.Aggregate;

public partial class Attendance
{
    /// <summary>Gets the boarding record associated with this attendance, if boarded.</summary>
    public BoardingRecord? BoardingRecord { get; private set; }

    /// <summary>Gets the absence record associated with this attendance, if absent.</summary>
    public AbsenceRecord? AbsenceRecord { get; private set; }

    /// <summary>
    ///     Marks the student as waiting for pickup and clears any previous drop-off timestamp.
    /// </summary>
    public void MarkWaiting()
    {
        Status = AttendanceStatus.Waiting;
        DroppedOffAt = null;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    ///     Marks the student as boarded and creates or updates the associated boarding record.
    /// </summary>
    /// <param name="location">The pickup location where the student boarded.</param>
    public void MarkBoarding(string location)
    {
        Status = AttendanceStatus.Boarded;
        DroppedOffAt = null;

        if (BoardingRecord is null)
            BoardingRecord = new BoardingRecord(location);
        else
            BoardingRecord.Record(location);

        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    ///     Marks the student as dropped off at the destination and records the drop-off timestamp.
    /// </summary>
    public void MarkDropOff()
    {
        Status = AttendanceStatus.DroppedOff;
        DroppedOffAt = DateTime.UtcNow;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    ///     Marks the student as absent and creates or updates the associated absence record.
    /// </summary>
    /// <param name="reason">The reason explaining the absence.</param>
    /// <param name="justified">Indicates whether the absence is justified.</param>
    public void MarkAbsent(string reason, bool justified)
    {
        Status = AttendanceStatus.Absent;
        DroppedOffAt = null;

        if (AbsenceRecord is null)
            AbsenceRecord = new AbsenceRecord(reason, justified);
        else
            AbsenceRecord.Register(reason, justified);

        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    ///     Marks the student as late for boarding.
    /// </summary>
    public void MarkLate()
    {
        Status = AttendanceStatus.Late;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    ///     Marks the attendance entry as pending manual confirmation.
    /// </summary>
    public void MarkPendingConfirmation()
    {
        Status = AttendanceStatus.PendingConfirmation;
        Timestamp = DateTime.UtcNow;
    }
}