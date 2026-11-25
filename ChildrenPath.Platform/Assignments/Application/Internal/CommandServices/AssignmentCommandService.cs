using ChildrenPath.Platform.Assignments.Application.CommandServices;
using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Domain.Model.Commands;
using ChildrenPath.Platform.Assignments.Domain.Model.Errors;
using ChildrenPath.Platform.Assignments.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Assignments.Application.Internal.CommandServices;

/// <summary>
///     Default implementation of <see cref="IAssignmentCommandService" />.
/// </summary>
/// <param name="assignmentRepository">The repository used to persist assignment aggregates.</param>
/// <param name="unitOfWork">The unit of work coordinating database transactions.</param>
public class AssignmentCommandService(
    IAssignmentRepository assignmentRepository,
    IUnitOfWork unitOfWork) : IAssignmentCommandService
{
    /// <inheritdoc />
    public async Task<Result<Assignment>> Handle(
        CreateAssignmentCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.StudentCode) ||
            string.IsNullOrWhiteSpace(command.StudentName) ||
            string.IsNullOrWhiteSpace(command.RouteCode) ||
            string.IsNullOrWhiteSpace(command.RouteName))
        {
            return Result<Assignment>.Failure(AssignmentErrors.InvalidAssignmentData);
        }

        if (!Enum.IsDefined(command.Shift))
            return Result<Assignment>.Failure(AssignmentErrors.InvalidShift);

        if (command.Validation.HasValue && !Enum.IsDefined(command.Validation.Value))
            return Result<Assignment>.Failure(AssignmentErrors.InvalidValidationState);

        if (command.Status.HasValue && !Enum.IsDefined(command.Status.Value))
            return Result<Assignment>.Failure(AssignmentErrors.InvalidStatus);

        var alreadyExists = await assignmentRepository.ExistsByStudentCodeAndShiftAsync(
            command.StudentCode,
            command.Shift,
            cancellationToken);

        if (alreadyExists)
            return Result<Assignment>.Failure(AssignmentErrors.AssignmentAlreadyExists);

        var assignment = new Assignment(
            command.StudentCode,
            command.StudentName,
            command.Grade ?? string.Empty,
            command.GuardianName ?? string.Empty,
            command.RouteCode,
            command.RouteName,
            command.VehiclePlate ?? string.Empty,
            command.DriverName ?? string.Empty,
            command.Shift,
            command.PickupPoint ?? string.Empty,
            command.PickupWindow ?? string.Empty,
            command.CapacityUsage,
            command.Notes ?? string.Empty,
            command.ValidationScore,
            command.Validation,
            command.Status);

        try
        {
            await assignmentRepository.AddAsync(assignment, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Assignment>.Success(assignment);
        }
        catch (OperationCanceledException)
        {
            return Result<Assignment>.Failure(AssignmentErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Assignment>.Failure(AssignmentErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Assignment>.Failure(AssignmentErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Assignment>> Handle(
        UpdateAssignmentCommand command,
        CancellationToken cancellationToken = default)
    {
        var assignment = await assignmentRepository.FindByIdAsync(command.Id, cancellationToken);
        if (assignment is null)
            return Result<Assignment>.Failure(AssignmentErrors.AssignmentNotFound);

        if (string.IsNullOrWhiteSpace(command.StudentCode) ||
            string.IsNullOrWhiteSpace(command.StudentName) ||
            string.IsNullOrWhiteSpace(command.RouteCode) ||
            string.IsNullOrWhiteSpace(command.RouteName))
        {
            return Result<Assignment>.Failure(AssignmentErrors.InvalidAssignmentData);
        }

        if (!Enum.IsDefined(command.Shift))
            return Result<Assignment>.Failure(AssignmentErrors.InvalidShift);

        if (!Enum.IsDefined(command.Validation))
            return Result<Assignment>.Failure(AssignmentErrors.InvalidValidationState);

        if (!Enum.IsDefined(command.Status))
            return Result<Assignment>.Failure(AssignmentErrors.InvalidStatus);

        var duplicateExists = await assignmentRepository.ExistsByStudentCodeAndShiftExcludingIdAsync(
            command.StudentCode,
            command.Shift,
            command.Id,
            cancellationToken);

        if (duplicateExists)
            return Result<Assignment>.Failure(AssignmentErrors.AssignmentAlreadyExists);

        assignment.UpdateDetails(
            command.StudentCode,
            command.StudentName,
            command.Grade ?? string.Empty,
            command.GuardianName ?? string.Empty,
            command.RouteCode,
            command.RouteName,
            command.VehiclePlate ?? string.Empty,
            command.DriverName ?? string.Empty,
            command.Shift,
            command.PickupPoint ?? string.Empty,
            command.PickupWindow ?? string.Empty,
            command.CapacityUsage,
            command.ValidationScore,
            command.Validation,
            command.Status,
            command.Notes ?? string.Empty);

        try
        {
            assignmentRepository.Update(assignment);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Assignment>.Success(assignment);
        }
        catch (OperationCanceledException)
        {
            return Result<Assignment>.Failure(AssignmentErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Assignment>.Failure(AssignmentErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Assignment>.Failure(AssignmentErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Assignment>> Handle(
        ValidateAssignmentCommand command,
        CancellationToken cancellationToken = default)
    {
        var assignment = await assignmentRepository.FindByIdAsync(command.AssignmentId, cancellationToken);
        if (assignment is null)
            return Result<Assignment>.Failure(AssignmentErrors.AssignmentNotFound);

        assignment.ValidateAssignment();

        try
        {
            assignmentRepository.Update(assignment);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Assignment>.Success(assignment);
        }
        catch (OperationCanceledException)
        {
            return Result<Assignment>.Failure(AssignmentErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Assignment>.Failure(AssignmentErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Assignment>.Failure(AssignmentErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Assignment>> Handle(
        MarkAssignmentConflictCommand command,
        CancellationToken cancellationToken = default)
    {
        var assignment = await assignmentRepository.FindByIdAsync(command.AssignmentId, cancellationToken);
        if (assignment is null)
            return Result<Assignment>.Failure(AssignmentErrors.AssignmentNotFound);

        if (!Enum.IsDefined(command.Validation))
            return Result<Assignment>.Failure(AssignmentErrors.InvalidValidationState);

        var reason = string.IsNullOrWhiteSpace(command.Reason)
            ? "Conflict detected in assignment."
            : command.Reason;

        assignment.MarkAsConflict(reason, command.Validation);

        try
        {
            assignmentRepository.Update(assignment);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Assignment>.Success(assignment);
        }
        catch (OperationCanceledException)
        {
            return Result<Assignment>.Failure(AssignmentErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Assignment>.Failure(AssignmentErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Assignment>.Failure(AssignmentErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<bool>> Handle(
        DeleteAssignmentCommand command,
        CancellationToken cancellationToken = default)
    {
        var assignment = await assignmentRepository.FindByIdAsync(command.AssignmentId, cancellationToken);
        if (assignment is null)
            return Result<bool>.Failure(AssignmentErrors.AssignmentNotFound);

        try
        {
            assignmentRepository.Remove(assignment);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (OperationCanceledException)
        {
            return Result<bool>.Failure(AssignmentErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<bool>.Failure(AssignmentErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<bool>.Failure(AssignmentErrors.InternalServerError);
        }
    }
}