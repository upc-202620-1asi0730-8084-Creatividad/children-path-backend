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

public class AttendanceCommandService(
    IAttendanceRepository attendanceRepository,
    IUnitOfWork unitOfWork) : IAttendanceCommandService
{
    public async Task<Result<AttendanceEntity>> Handle(
        CreateAttendanceCommand command,
        CancellationToken cancellationToken)
    {
        var exists = await attendanceRepository.ExistsByStudentIdAndTripIdAsync(
            command.StudentId,
            command.TripId,
            cancellationToken);

        if (exists)
            return Result<AttendanceEntity>.Failure(AttendanceErrors.AttendanceAlreadyExists);

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
        catch (OperationCanceledException) { return Result<AttendanceEntity>.Failure(AttendanceErrors.OperationCancelled); }
        catch (DbUpdateException)          { return Result<AttendanceEntity>.Failure(AttendanceErrors.DatabaseError); }
        catch (Exception)                  { return Result<AttendanceEntity>.Failure(AttendanceErrors.InternalServerError); }
    }

    public async Task<Result<AttendanceEntity>> Handle(
        UpdateAttendanceStatusCommand command,
        CancellationToken cancellationToken)
    {
        var attendance = await attendanceRepository.FindByIdAsync(command.AttendanceId, cancellationToken);
        if (attendance is null)
            return Result<AttendanceEntity>.Failure(AttendanceErrors.AttendanceNotFound);

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
                return Result<AttendanceEntity>.Failure(AttendanceErrors.InvalidStatus);
        }

        try
        {
            attendanceRepository.Update(attendance);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<AttendanceEntity>.Success(attendance);
        }
        catch (OperationCanceledException) { return Result<AttendanceEntity>.Failure(AttendanceErrors.OperationCancelled); }
        catch (DbUpdateException)          { return Result<AttendanceEntity>.Failure(AttendanceErrors.DatabaseError); }
        catch (Exception)                  { return Result<AttendanceEntity>.Failure(AttendanceErrors.InternalServerError); }
    }

    public async Task<Result<AttendanceEntity>> Handle(
        MarkDropOffCommand command,
        CancellationToken cancellationToken)
    {
        var attendance = await attendanceRepository.FindByIdAsync(command.AttendanceId, cancellationToken);
        if (attendance is null)
            return Result<AttendanceEntity>.Failure(AttendanceErrors.AttendanceNotFound);

        attendance.MarkDropOff();

        try
        {
            attendanceRepository.Update(attendance);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<AttendanceEntity>.Success(attendance);
        }
        catch (OperationCanceledException) { return Result<AttendanceEntity>.Failure(AttendanceErrors.OperationCancelled); }
        catch (DbUpdateException)          { return Result<AttendanceEntity>.Failure(AttendanceErrors.DatabaseError); }
        catch (Exception)                  { return Result<AttendanceEntity>.Failure(AttendanceErrors.InternalServerError); }
    }
}