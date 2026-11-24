using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Fleet.Domain.Repositories;

public interface IMaintenanceAlertRepository : IBaseRepository<MaintenanceAlert>
{
    Task<IEnumerable<MaintenanceAlert>> FindByVehicleCodeAsync(string vehicleCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaintenanceAlert>> FindPendingAlertsAsync(CancellationToken cancellationToken = default);
}
