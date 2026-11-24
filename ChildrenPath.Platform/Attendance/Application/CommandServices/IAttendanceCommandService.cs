using ChildrenPath.Platform.Attendance.Domain.Model.Commands;
using ChildrenPath.Platform.Shared.Domain;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Application.CommandServices;

/// <summary>
///     Represents the command service contract for mutating attendance records.
/// </summary>
public interface IAttendanceCommandService
{
    /// <summary>
    ///     Handles the creation of a new attendance record.
    /// </summary>
    /// <param name="command">The <see cref="CreateAttendanceCommand" /> command.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Result{T}" /> containing the created attendance record or an error.</returns>
    Task<Result<AttendanceEntity>> Handle(CreateAttendanceCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles updating the status of an existing attendance record.
    /// </summary>
    /// <param name="command">The <see cref="UpdateAttendanceStatusCommand" /> command.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Result{T}" /> containing the updated attendance record or an error.</returns>
    Task<Result<AttendanceEntity>> Handle(UpdateAttendanceStatusCommand command, CancellationToken cancellationToken);

    /// <summary>
    ///     Handles marking an attendance record as dropped off.
    /// </summary>
    /// <param name="command">The <see cref="MarkDropOffCommand" /> command.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Result{T}" /> containing the updated attendance record or an error.</returns>
    Task<Result<AttendanceEntity>> Handle(MarkDropOffCommand command, CancellationToken cancellationToken);
}