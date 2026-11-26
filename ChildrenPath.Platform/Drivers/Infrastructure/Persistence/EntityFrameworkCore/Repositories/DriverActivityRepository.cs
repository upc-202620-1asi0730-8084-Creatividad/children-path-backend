using ChildrenPath.Platform.Drivers.Domain.Model.Entities;
using ChildrenPath.Platform.Drivers.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Drivers.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class DriverActivityRepository(AppDbContext context)
    : BaseRepository<DriverActivity>(context), IDriverActivityRepository
{
    public async Task<IEnumerable<DriverActivity>> ListRecentAsync(
        int limit,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<DriverActivity>()
            .AsNoTracking()
            .OrderByDescending(activity => activity.OccurredAt)
            .ThenByDescending(activity => activity.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}