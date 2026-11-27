using ChildrenPath.Platform.Subscriptions.Application.QueryServices;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Queries;
using ChildrenPath.Platform.Subscriptions.Domain.Repositories;

namespace ChildrenPath.Platform.Subscriptions.Application.Internal.QueryServices;

/// <summary>
///     Default implementation of <see cref="ISubscriptionQueryService" />.
/// </summary>
/// <param name="subscriptionRepository">The repository used to query subscriptions.</param>
public class SubscriptionQueryService(ISubscriptionRepository subscriptionRepository) : ISubscriptionQueryService
{
    /// <inheritdoc />
    public async Task<IEnumerable<Subscription>> Handle(
        GetAllSubscriptionsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await subscriptionRepository.ListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Subscription?> Handle(
        GetSubscriptionByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await subscriptionRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Subscription?> Handle(
        GetCurrentSubscriptionQuery query,
        CancellationToken cancellationToken = default)
    {
        return await subscriptionRepository.FindLatestActiveAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Subscription?> Handle(
        GetActiveSubscriptionByUserIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await subscriptionRepository.FindActiveByUserIdAsync(query.UserId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Subscription>> Handle(
        GetSubscriptionsByUserIdQuery query,
        CancellationToken cancellationToken = default)
    {
        return await subscriptionRepository.FindByUserIdAsync(query.UserId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Subscription>> Handle(
        GetSubscriptionsByStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        return await subscriptionRepository.FindByStatusAsync(query.Status, cancellationToken);
    }
}