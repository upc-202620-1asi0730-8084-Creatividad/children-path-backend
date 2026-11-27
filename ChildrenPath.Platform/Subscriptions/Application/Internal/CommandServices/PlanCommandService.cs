using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Subscriptions.Application.CommandServices;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Errors;
using ChildrenPath.Platform.Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Subscriptions.Application.Internal.CommandServices;

/// <summary>
///     Default implementation of <see cref="IPlanCommandService" />.
/// </summary>
/// <param name="planRepository">The repository used to persist subscription plans.</param>
/// <param name="unitOfWork">The unit of work coordinating database transactions.</param>
public class PlanCommandService(
    IPlanRepository planRepository,
    IUnitOfWork unitOfWork) : IPlanCommandService
{
    /// <inheritdoc />
    public async Task<Result<Plan>> Handle(
        CreatePlanCommand command,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Code) || string.IsNullOrWhiteSpace(command.Name))
            return Result<Plan>.Failure(SubscriptionErrors.InvalidPlanData);

        if (!Enum.IsDefined(command.Type))
            return Result<Plan>.Failure(SubscriptionErrors.InvalidPlanType);

        var codeExists = await planRepository.ExistsByCodeAsync(command.Code, cancellationToken);
        if (codeExists)
            return Result<Plan>.Failure(SubscriptionErrors.PlanCodeAlreadyExists);

        var plan = new Plan(
            command.Code,
            command.Name,
            command.Price,
            command.Type,
            command.VehicleLimit,
            command.StudentLimit,
            command.UserLimit,
            command.Currency,
            command.Interval,
            command.IsFeatured,
            command.FeaturesJson);

        try
        {
            await planRepository.AddAsync(plan, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Plan>.Success(plan);
        }
        catch (OperationCanceledException)
        {
            return Result<Plan>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Plan>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Plan>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Plan>> Handle(
        UpdatePlanCommand command,
        CancellationToken cancellationToken = default)
    {
        var plan = await planRepository.FindByIdAsync(command.PlanId, cancellationToken);
        if (plan is null)
            return Result<Plan>.Failure(SubscriptionErrors.PlanNotFound);

        if (string.IsNullOrWhiteSpace(command.Name))
            return Result<Plan>.Failure(SubscriptionErrors.InvalidPlanData);

        if (!Enum.IsDefined(command.Type))
            return Result<Plan>.Failure(SubscriptionErrors.InvalidPlanType);

        plan.UpdateDetails(
            command.Name,
            command.Price,
            command.Type,
            command.VehicleLimit,
            command.StudentLimit,
            command.UserLimit,
            command.IsFeatured,
            command.FeaturesJson);

        try
        {
            planRepository.Update(plan);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Plan>.Success(plan);
        }
        catch (OperationCanceledException)
        {
            return Result<Plan>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Plan>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Plan>.Failure(SubscriptionErrors.InternalServerError);
        }
    }

    /// <inheritdoc />
    public async Task<Result<Plan>> Handle(
        TogglePlanStatusCommand command,
        CancellationToken cancellationToken = default)
    {
        var plan = await planRepository.FindByIdAsync(command.PlanId, cancellationToken);
        if (plan is null)
            return Result<Plan>.Failure(SubscriptionErrors.PlanNotFound);

        if (command.IsActive)
            plan.Activate();
        else
            plan.Deactivate();

        try
        {
            planRepository.Update(plan);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<Plan>.Success(plan);
        }
        catch (OperationCanceledException)
        {
            return Result<Plan>.Failure(SubscriptionErrors.OperationCancelled);
        }
        catch (DbUpdateException)
        {
            return Result<Plan>.Failure(SubscriptionErrors.DatabaseError);
        }
        catch (Exception)
        {
            return Result<Plan>.Failure(SubscriptionErrors.InternalServerError);
        }
    }
}