using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;
using ChildrenPath.Platform.Subscriptions.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Subscriptions.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <summary>
///     Entity Framework Core implementation of <see cref="IPlanRepository" />.
/// </summary>
/// <param name="context">The application <see cref="AppDbContext" />.</param>
public class PlanRepository(AppDbContext context)
    : BaseRepository<Plan>(context), IPlanRepository
{
    /// <inheritdoc />
    public override async Task<IEnumerable<Plan>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Plan>()
            .OrderBy(p => p.Type)
            .ThenBy(p => p.Id)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Plan?> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToLowerInvariant();
        return await Context.Set<Plan>()
            .FirstOrDefaultAsync(p => p.Code.ToLower() == normalizedCode, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToLowerInvariant();
        return await Context.Set<Plan>()
            .AnyAsync(p => p.Code.ToLower() == normalizedCode, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByCodeExcludingIdAsync(
        string code,
        int excludeId,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToLowerInvariant();
        return await Context.Set<Plan>()
            .AnyAsync(p => p.Id != excludeId && p.Code.ToLower() == normalizedCode, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Plan>> FindActivePlansAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<Plan>()
            .Where(p => p.IsActive)
            .OrderBy(p => p.Type)
            .ThenBy(p => p.Id)
            .ToListAsync(cancellationToken);
    }
}