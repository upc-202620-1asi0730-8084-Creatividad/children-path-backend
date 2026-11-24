using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class MaintenanceRecordRepository(AppDbContext context) : BaseRepository<MaintenanceRecord>(context), IMaintenanceRecordRepository
{
    public async Task<IEnumerable<MaintenanceRecord>> FindByVehicleIdAsync(int vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.MaintenanceRecords
            .Where(m => m.VehicleId == vehicleId)
            .ToListAsync(cancellationToken);
    }
}
