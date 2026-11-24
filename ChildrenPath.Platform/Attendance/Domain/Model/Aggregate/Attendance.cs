using ChildrenPath.Platform.Attendance.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Attendance.Domain.Model.Aggregate;
/// <summary>
///     Represents the Attendance aggregate root within the Attendance Bounded Context.
/// </summary>
/// <remarks>
///     Encapsulates the attendance state of a student for a specific trip, maintaining
///     consistency across its contained <see cref="Entities.BoardingRecord" /> and
///     <see cref="Entities.AbsenceRecord" /> entities.
/// </remarks>
public partial class Attendance
{
    /// <summary>
    ///     Default constructor used by Entity Framework Core persistence.
    /// </summary>
    public Attendance() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Attendance" /> aggregate.
    /// </summary>
    /// <param name="studentId">The identifier of the student being tracked.</param>
    /// <param name="tripId">The identifier of the trip this attendance belongs to.</param>
    public Attendance(int studentId, int tripId)
    {
        StudentId = studentId;
        TripId = tripId;
        Timestamp = DateTime.UtcNow;
        Status = AttendanceStatus.Waiting;
    }

    /// <summary>Gets or sets the unique identifier of the attendance record.</summary>
    public int Id { get; set; }

    /// <summary>Gets the identifier of the student.</summary>
    public int StudentId { get; private set; }

    /// <summary>Gets the identifier of the trip.</summary>
    public int TripId { get; private set; }

    /// <summary>Gets the UTC timestamp of the latest status update.</summary>
    public DateTime Timestamp { get; private set; } = DateTime.UtcNow;

    /// <summary>Gets the UTC timestamp when the student was dropped off, if applicable.</summary>
    public DateTime? DroppedOffAt { get; private set; }

    /// <summary>Gets the current attendance status of the student.</summary>
    public AttendanceStatus Status { get; private set; } = AttendanceStatus.Waiting;
}