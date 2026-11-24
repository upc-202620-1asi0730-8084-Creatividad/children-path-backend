namespace ChildrenPath.Platform.Attendance.Domain.Model.Entities;

/// <summary>
///     Represents the boarding record for an attendance entry.
/// </summary>
public class BoardingRecord
{
    /// <summary>
    ///     Default constructor used by Entity Framework Core persistence.
    /// </summary>
    public BoardingRecord() { }
    /// <summary>
    ///     Initializes a new instance of the <see cref="BoardingRecord" /> entity.
    /// </summary>
    /// <param name="location">The pickup location where the student boarded the vehicle.</param>

    public BoardingRecord(string location)
    {
        Location = location;
        BoardedAt = DateTime.UtcNow;
    }
    /// <summary>Gets or sets the unique identifier of the boarding record.</summary>
    public int Id { get; set; }

    /// <summary>Gets or sets the foreign key referencing the parent attendance record.</summary>
    public int AttendanceId { get; set; }

    /// <summary>Gets the UTC timestamp when the student boarded the vehicle.</summary>
    public DateTime BoardedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>Gets the location where the boarding event was registered.</summary>
    public string Location { get; private set; } = string.Empty;

    /// <summary>
    ///     Updates the boarding location and refreshes the boarding timestamp.
    /// </summary>
    /// <param name="location">The updated boarding location.</param>
    public void Record(string location)
    {
        Location = location;
        BoardedAt = DateTime.UtcNow;
    }
}