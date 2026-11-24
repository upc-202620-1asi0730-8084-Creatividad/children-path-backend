namespace ChildrenPath.Platform.Attendance.Interfaces.Resources;

/// <summary>
///     Represents an attendance record returned by the REST API.
/// </summary>
/// <param name="Id">The unique identifier of the attendance record.</param>
/// <param name="StudentId">The identifier of the student.</param>
/// <param name="TripId">The identifier of the trip.</param>
/// <param name="Status">The current attendance status.</param>
/// <param name="Timestamp">The UTC timestamp of the last status update.</param>
/// <param name="DroppedOffAt">The UTC timestamp when the student was dropped off, if applicable.</param>
/// <param name="BoardingRecord">The boarding detail record, if available.</param>
/// <param name="AbsenceRecord">The absence detail record, if available.</param>
public record AttendanceResource(
    int Id,
    int StudentId,
    int TripId,
    string Status,
    DateTime Timestamp,
    DateTime? DroppedOffAt,
    BoardingRecordResource? BoardingRecord,
    AbsenceRecordResource? AbsenceRecord);

/// <summary>
///     Represents the boarding details of an attendance record.
/// </summary>
/// <param name="Id">The unique identifier of the boarding record.</param>
/// <param name="BoardedAt">The UTC timestamp when boarding occurred.</param>
/// <param name="Location">The pickup location where the student boarded.</param>
public record BoardingRecordResource(
    int Id,
    DateTime BoardedAt,
    string Location);

/// <summary>
///     Represents the absence details of an attendance record.
/// </summary>
/// <param name="Id">The unique identifier of the absence record.</param>
/// <param name="Reason">The reason for the absence.</param>
/// <param name="Justified">Indicates whether the absence is justified.</param>
public record AbsenceRecordResource(
    int Id,
    string Reason,
    bool Justified);