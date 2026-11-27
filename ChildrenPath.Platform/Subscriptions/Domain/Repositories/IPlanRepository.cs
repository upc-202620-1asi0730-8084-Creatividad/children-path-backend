using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;

namespace ChildrenPath.Platform.Subscriptions.Domain.Repositories;

/// <summary>
///     Repository contract for persisting and querying <see cref="Plan" /> aggregates.
/// </summary>
public interface IPlanRepository : IBaseRepository<Plan>
{
    /// <summary>
    ///     Retrieves a subscription plan by its unique code (e.g., starter, pro, enterprise).
    /// </summary>
    Task<Plan?> FindByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks whether a plan with the specified code already exists.
    /// </summary>
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Checks whether another plan (excluding a given id) already uses the specified code.
    /// </summary>
    Task<bool> ExistsByCodeExcludingIdAsync(
        string code,
        int excludeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all active plans ordered for display.
    /// </summary>
    Task<IEnumerable<Plan>> FindActivePlansAsync(CancellationToken cancellationToken = default);
}