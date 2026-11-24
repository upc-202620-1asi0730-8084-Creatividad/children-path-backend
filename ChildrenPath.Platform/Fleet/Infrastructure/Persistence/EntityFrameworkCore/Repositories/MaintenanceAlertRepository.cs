using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MaintenanceAlertRepository(AppDbContext context) : BaseRepository<MaintenanceAlert>(context), IMaintenanceAlertRepository
{
    public async Task<IEnumerable<MaintenanceAlert>> FindByVehicleCodeAsync(string vehicleCode, CancellationToken cancellationToken = default)
    {
        return await Context.Set<MaintenanceAlert>()
            .Where(a => a.VehicleCode == vehicleCode)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<MaintenanceAlert>> FindPendingAlertsAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<MaintenanceAlert>()
            .Where(a => !a.IsResolved)
            .ToListAsync(cancellationToken);
    }
}
