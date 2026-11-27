using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Repositories;

/// <summary>
///     Repository contract for persisting and querying <see cref="Subscription" /> aggregates
///     together with their plan, invoices, payments, and saved payment methods.
/// </summary>
public interface ISubscriptionRepository : IBaseRepository<Subscription>
{
    /// <summary>
    ///     Retrieves the active subscription for a specific user, including plan, invoices, payments, and payment methods.
    /// </summary>
    Task<Subscription?> FindActiveByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves the most recent active subscription (or the latest available subscription as fallback)
    ///     for the frontend billing dashboard endpoints.
    /// </summary>
    Task<Subscription?> FindLatestActiveAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all subscriptions belonging to a specific user.
    /// </summary>
    Task<IEnumerable<Subscription>> FindByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all subscriptions matching a specific lifecycle status.
    /// </summary>
    Task<IEnumerable<Subscription>> FindByStatusAsync(
        SubscriptionStatus status,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks whether a user already has an active subscription.
    /// </summary>
    Task<bool> ExistsActiveByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}