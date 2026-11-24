using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Fleet.Domain.Repositories;

public interface IMaintenanceRecordRepository : IBaseRepository<MaintenanceRecord>
{
    Task<IEnumerable<MaintenanceRecord>> FindByVehicleIdAsync(int vehicleId, CancellationToken cancellationToken = default);
}
