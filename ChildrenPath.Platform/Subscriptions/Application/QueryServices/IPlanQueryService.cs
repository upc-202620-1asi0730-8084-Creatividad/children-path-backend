using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;

namespace ChildrenPath.Platform.Subscriptions.Application.QueryServices;

/// <summary>
///     Application query service contract for reading <see cref="Plan" /> aggregates.
/// </summary>
public interface IPlanQueryService
{
    Task<IEnumerable<Plan>> Handle(GetAllPlansQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Plan>> Handle(GetActivePlansQuery query, CancellationToken cancellationToken = default);
    Task<Plan?> Handle(GetPlanByIdQuery query, CancellationToken cancellationToken = default);
    Task<Plan?> Handle(GetPlanByCodeQuery query, CancellationToken cancellationToken = default);
}