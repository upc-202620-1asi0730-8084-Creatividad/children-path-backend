using ChildrenPath.Platform.Attendance.Application.CommandServices;
using ChildrenPath.Platform.Attendance.Domain.Model.Commands;
using ChildrenPath.Platform.Attendance.Domain.Model.Errors;
using ChildrenPath.Platform.Attendance.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Attendance.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Application.Internal.CommandServices;

/// <summary>
///     Default implementation of <see cref="IAttendanceCommandService" />.
/// </summary>
/// <param name="attendanceRepository">The repository used to persist attendance records.</param>
/// <param name="unitOfWork">The unit of work used to commit persistence changes.</param>
public class AttendanceCommandService(
    IAttendanceRepository attendanceRepository,
    IUnitOfWork unitOfWork) : IAttendanceCommandService
{
    /// <inheritdoc />
    public async Task<Result<AttendanceEntity>> Handle(
        CreateAttendanceCommand command,
        CancellationToken cancellationToken)
    {
        var exists = await attendanceRepository.ExistsByStudentIdAndTripIdAsync(
            command.StudentId,
            command.TripId,
            cancellationToken);

        if (exists)
            return AttendanceErrors.AttendanceAlreadyExists.ToFailure<AttendanceEntity>();

        var attendance = new AttendanceEntity(command.StudentId, command.TripId);

        if (!string.IsNullOrWhiteSpace(command.AbsenceReason))
            attendance.MarkAbsent(command.AbsenceReason, command.IsAbsenceJustified);
        else if (!string.IsNullOrWhiteSpace(command.Location))
            attendance.MarkBoarding(command.Location);

        try
        {
            await attendanceRepository.AddAsync(attendance, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<AttendanceEntity>.Success(attendance);
        }
        catch (OperationCanceledException)
        {
            return AttendanceErrors.OperationCancelled.ToFailure<AttendanceEntity>();
        }
        catch (DbUpdateException)
        {
            return AttendanceErrors.DatabaseError.ToFailure<AttendanceEntity>();
        }
        catch (Exception)
        {
            return AttendanceErrors.InternalServerError.ToFailure<AttendanceEntity>();
        }
    }

    /// <inheritdoc />
    public async Task<Result<AttendanceEntity>> Handle(
        UpdateAttendanceStatusCommand command,
        CancellationToken cancellationToken)
    {
        var attendance = await attendanceRepository.FindByIdAsync(command.AttendanceId, cancellationToken);
        if (attendance is null)
            return AttendanceErrors.AttendanceNotFound.ToFailure<AttendanceEntity>();

        switch (command.Status)
        {
            case AttendanceStatus.Waiting:
                attendance.MarkWaiting();
                break;
            case AttendanceStatus.Boarded:
                attendance.MarkBoarding(
                    string.IsNullOrWhiteSpace(command.Location) ? "Confirmed boarding" : command.Location);
                break;
            case AttendanceStatus.DroppedOff:
                attendance.MarkDropOff();
                break;
            case AttendanceStatus.Absent:
                attendance.MarkAbsent(
                    string.IsNullOrWhiteSpace(command.AbsenceReason) ? "Absence registered" : command.AbsenceReason,
                    command.IsAbsenceJustified);
                break;
            case AttendanceStatus.Late:
                attendance.MarkLate();
                break;
            case AttendanceStatus.PendingConfirmation:
                attendance.MarkPendingConfirmation();
                break;
            default:
                return AttendanceErrors.InvalidStatus.ToFailure<AttendanceEntity>();
        }

        try
        {
            attendanceRepository.Update(attendance);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<AttendanceEntity>.Success(attendance);
        }
        catch (OperationCanceledException)
        {
            return AttendanceErrors.OperationCancelled.ToFailure<AttendanceEntity>();
        }
        catch (DbUpdateException)
        {
            return AttendanceErrors.DatabaseError.ToFailure<AttendanceEntity>();
        }
        catch (Exception)
        {
            return AttendanceErrors.InternalServerError.ToFailure<AttendanceEntity>();
        }
    }

    /// <inheritdoc />
    public async Task<Result<AttendanceEntity>> Handle(
        MarkDropOffCommand command,
        CancellationToken cancellationToken)
    {
        var attendance = await attendanceRepository.FindByIdAsync(command.AttendanceId, cancellationToken);
        if (attendance is null)
            return AttendanceErrors.AttendanceNotFound.ToFailure<AttendanceEntity>();

        attendance.MarkDropOff();

        try
        {
            attendanceRepository.Update(attendance);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<AttendanceEntity>.Success(attendance);
        }
        catch (OperationCanceledException)
        {
            return AttendanceErrors.OperationCancelled.ToFailure<AttendanceEntity>();
        }
        catch (DbUpdateException)
        {
            return AttendanceErrors.DatabaseError.ToFailure<AttendanceEntity>();
        }
        catch (Exception)
        {
            return AttendanceErrors.InternalServerError.ToFailure<AttendanceEntity>();
        }
    }
}