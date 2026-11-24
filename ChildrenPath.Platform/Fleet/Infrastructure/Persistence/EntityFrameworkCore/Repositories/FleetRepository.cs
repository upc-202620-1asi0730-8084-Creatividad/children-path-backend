using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class FleetRepository(AppDbContext context) : BaseRepository<TransportFleet>(context), IFleetRepository
{
    public async Task<IEnumerable<TransportFleet>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await Context.Fleets
            .Where(f => f.CompanyId == companyId)
            .Include(f => f.Vehicles)
            .ToListAsync(cancellationToken);
    }
}
