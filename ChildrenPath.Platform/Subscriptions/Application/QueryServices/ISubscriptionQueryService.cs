using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;

namespace ChildrenPath.Platform.Subscriptions.Application.QueryServices;

/// <summary>
///     Application query service contract for reading <see cref="Subscription" /> aggregates.
/// </summary>
public interface ISubscriptionQueryService
{
    Task<IEnumerable<Subscription>> Handle(GetAllSubscriptionsQuery query, CancellationToken cancellationToken = default);
    Task<Subscription?> Handle(GetSubscriptionByIdQuery query, CancellationToken cancellationToken = default);
    Task<Subscription?> Handle(GetCurrentSubscriptionQuery query, CancellationToken cancellationToken = default);
    Task<Subscription?> Handle(GetActiveSubscriptionByUserIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subscription>> Handle(GetSubscriptionsByUserIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Subscription>> Handle(GetSubscriptionsByStatusQuery query, CancellationToken cancellationToken = default);
}