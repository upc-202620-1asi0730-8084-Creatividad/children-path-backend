using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Fleet.Domain.Repositories;

public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    Task<Vehicle?> FindByPlateAsync(string plate, CancellationToken cancellationToken = default);
    Task<Vehicle?> FindByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<bool> ExistsByPlateAsync(string plate, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> FindByStatusAsync(string status, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vehicle>> FindByFleetIdAsync(int fleetId, CancellationToken cancellationToken = default);
}
