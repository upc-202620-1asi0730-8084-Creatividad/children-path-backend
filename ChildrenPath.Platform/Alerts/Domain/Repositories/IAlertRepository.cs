using ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;
using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Alerts.Domain.Repositories;

public interface IAlertRepository : IBaseRepository<Alert>
{
    Task<IEnumerable<Alert>> FindByTripIdAsync(int tripId, CancellationToken ct = default);
    Task<IEnumerable<Alert>> FindByStatusAsync(AlertStatus status, CancellationToken ct = default);
    Task<bool> ExistsByTripIdAndTypeAsync(int tripId, AlertType type, CancellationToken ct = default);
    
    Task<IEnumerable<Alert>> FindAllActiveAsync(CancellationToken ct = default);
}