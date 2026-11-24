using ChildrenPath.Platform.Attendance.Domain.Model.Commands;
using ChildrenPath.Platform.Attendance.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Attendance.Interfaces.Resources;

namespace ChildrenPath.Platform.Attendance.Interfaces.Transform;

/// <summary>
///     Assembles <see cref="UpdateAttendanceStatusCommand" /> instances from <see cref="UpdateAttendanceStatusResource" /> payloads.
/// </summary>
public static class UpdateAttendanceStatusCommandFromResourceAssembler
{
    /// <summary>
    ///     Maps an attendance identifier and <see cref="UpdateAttendanceStatusResource" /> into an <see cref="UpdateAttendanceStatusCommand" />.
    /// </summary>
    /// <param name="attendanceId">The identifier of the attendance record being updated.</param>
    /// <param name="resource">The incoming status update payload.</param>
    /// <returns>The corresponding <see cref="UpdateAttendanceStatusCommand" />.</returns>
    public static UpdateAttendanceStatusCommand ToCommandFromResource(
        int attendanceId,
        UpdateAttendanceStatusResource resource)
    {
        var normalized = resource.Status?.Trim().ToLowerInvariant() ?? string.Empty;

        var status = normalized switch
        {
            "waiting" => AttendanceStatus.Waiting,
            "on_board" or "boarded" => AttendanceStatus.Boarded,
            "arrived" or "droppedoff" or "dropped_off" => AttendanceStatus.DroppedOff,
            "absent" => AttendanceStatus.Absent,
            "late" => AttendanceStatus.Late,
            "pending_confirmation" or "pendingconfirmation" => AttendanceStatus.PendingConfirmation,
            _ => (AttendanceStatus)(-1)
        };

        var location = !string.IsNullOrWhiteSpace(resource.Location)
            ? resource.Location
            : resource.PickupPoint;

        var absenceReason = !string.IsNullOrWhiteSpace(resource.AbsenceReason)
            ? resource.AbsenceReason
            : resource.Notes;

        return new UpdateAttendanceStatusCommand(
            attendanceId,
            status,
            location,
            absenceReason,
            resource.IsAbsenceJustified);
    }
}