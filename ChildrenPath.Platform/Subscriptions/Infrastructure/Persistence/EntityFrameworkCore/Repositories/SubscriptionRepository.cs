using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Subscriptions.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core implementation of <see cref="ISubscriptionRepository" />.
/// </summary>
/// <param name="context">The application <see cref="AppDbContext" />.</param>
public class SubscriptionRepository(AppDbContext context)
    : BaseRepository<Subscription>(context), ISubscriptionRepository
{
    private IQueryable<Subscription> QueryWithIncludes()
    {
        return Context.Set<Subscription>()
            .Include(s => s.Plan)
            .Include(s => s.Invoices)
                .ThenInclude(i => i.Payments)
            .Include(s => s.PaymentMethods);
    }

    /// <inheritdoc />
    public override async Task<Subscription?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await QueryWithIncludes()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<IEnumerable<Subscription>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await QueryWithIncludes()
            .OrderByDescending(s => s.StartDate)
            .ThenByDescending(s => s.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Subscription?> FindActiveByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await QueryWithIncludes()
            .Where(s => s.UserId == userId && s.Status == SubscriptionStatus.Active)
            .OrderByDescending(s => s.StartDate)
            .ThenByDescending(s => s.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Subscription?> FindLatestActiveAsync(CancellationToken cancellationToken = default)
    {
        var active = await QueryWithIncludes()
            .Where(s => s.Status == SubscriptionStatus.Active)
            .OrderByDescending(s => s.StartDate)
            .ThenByDescending(s => s.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (active is not null)
            return active;

        return await QueryWithIncludes()
            .OrderByDescending(s => s.StartDate)
            .ThenByDescending(s => s.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Subscription>> FindByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await QueryWithIncludes()
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.StartDate)
            .ThenByDescending(s => s.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Subscription>> FindByStatusAsync(
        SubscriptionStatus status,
        CancellationToken cancellationToken = default)
    {
        return await QueryWithIncludes()
            .Where(s => s.Status == status)
            .OrderByDescending(s => s.StartDate)
            .ThenByDescending(s => s.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsActiveByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Subscription>()
            .AnyAsync(s => s.UserId == userId && s.Status == SubscriptionStatus.Active, cancellationToken);
    }
}