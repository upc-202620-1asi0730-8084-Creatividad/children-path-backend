namespace ChildrenPath.Platform.Attendance.Interfaces.Resources;

/// <summary>
///     Request payload for updating the status of an attendance record.
/// </summary>
/// <param name="Status">The target status (supports backend enum names and frontend status keys).</param>
/// <param name="Location">The optional boarding location.</param>
/// <param name="AbsenceReason">The optional absence reason.</param>
/// <param name="IsAbsenceJustified">Indicates whether the absence is justified.</param>
/// <param name="PickupPoint">Optional pickup point alias sent by the frontend.</param>
/// <param name="Notes">Optional notes alias sent by the frontend.</param>
public record UpdateAttendanceStatusResource(
    string Status,
    string? Location = null,
    string? AbsenceReason = null,
    bool IsAbsenceJustified = false,
    string? PickupPoint = null,
    string? Notes = null);