namespace ChildrenPath.Platform.Attendance.Domain.Model.Entities;

/// <summary>
///     Represents an absence detail entry for an attendance record.
/// </summary>
public class AbsenceRecord
{
    /// <summary>
    ///     Default constructor used by Entity Framework Core persistence.
    /// </summary>
    public AbsenceRecord() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AbsenceRecord" /> entity.
    /// </summary>
    /// <param name="reason">The reason explaining the student's absence.</param>
    /// <param name="justified">Indicates whether the absence is justified.</param>
    public AbsenceRecord(string reason, bool justified)
    {
        Reason = reason;
        Justified = justified;
    }

    /// <summary>Gets or sets the unique identifier of the absence record.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the foreign key referencing the parent attendance record.</summary>
    public int AttendanceId { get; set; }

    /// <summary>Gets the reason provided for the absence.</summary>
    public string Reason { get; private set; } = string.Empty;

    /// <summary>Gets a value indicating whether the absence has been justified.</summary>
    public bool Justified { get; private set; }

    /// <summary>
    ///     Registers or updates the absence details.
    /// </summary>
    /// <param name="reason">The reason for the absence.</param>
    /// <param name="justified">Indicates whether the absence is justified.</param>
    public void Register(string reason, bool justified)
    {
        Reason = reason;
        Justified = justified;
    }
}