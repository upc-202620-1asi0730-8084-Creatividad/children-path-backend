using ChildrenPath.Platform.Subscriptions.Application.QueryServices;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;
using ChildrenPath.Platform.Subscriptions.Domain.Repositories;

namespace ChildrenPath.Platform.Subscriptions.Application.Internal.QueryServices;

/// <summary>
///     Default implementation of <see cref="IPlanQueryService" />.
/// </summary>
/// <param name="planRepository">The repository used to query subscription plans.</param>
public class PlanQueryService(IPlanRepository planRepository) : IPlanQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Plan>> Handle(
        GetAllPlansQuery query,
        CancellationToken cancellationToken = default)
    {
        return await planRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Plan>> Handle(
        GetActivePlansQuery query,
        CancellationToken cancellationToken = default)
    {
        return await planRepository.FindActivePlansAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Plan?> Handle(
        GetPlanByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await planRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Plan?> Handle(
        GetPlanByCodeQuery query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.Code))
            return null;

        return await planRepository.FindByCodeAsync(query.Code, cancellationToken);
    }
}