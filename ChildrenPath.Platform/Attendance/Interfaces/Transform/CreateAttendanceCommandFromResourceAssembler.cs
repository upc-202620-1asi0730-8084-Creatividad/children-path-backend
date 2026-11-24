using ChildrenPath.Platform.Attendance.Domain.Model.Commands;
using ChildrenPath.Platform.Attendance.Interfaces.Resources;

namespace ChildrenPath.Platform.Attendance.Interfaces.Transform;

/// <summary>
///     Assembles <see cref="CreateAttendanceCommand" /> instances from <see cref="CreateAttendanceResource" /> payloads.
/// </summary>
public static class CreateAttendanceCommandFromResourceAssembler
{
    /// <summary>
    ///     Maps a <see cref="CreateAttendanceResource" /> into a <see cref="CreateAttendanceCommand" />.
    /// </summary>
    /// <param name="resource">The incoming create request resource.</param>
    /// <returns>The corresponding <see cref="CreateAttendanceCommand" />.</returns>
    public static CreateAttendanceCommand ToCommandFromResource(CreateAttendanceResource resource)
    {
        return new CreateAttendanceCommand(
            resource.StudentId,
            resource.TripId,
            resource.Location,
            resource.AbsenceReason,
            resource.IsAbsenceJustified);
    }
}