using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Fleet.Domain.Repositories;

public interface IVehicleDocumentRepository : IBaseRepository<VehicleDocument>
{
    Task<IEnumerable<VehicleDocument>> FindByVehicleIdAsync(int vehicleId, CancellationToken cancellationToken = default);
}
