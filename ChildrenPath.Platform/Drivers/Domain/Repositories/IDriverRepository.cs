using ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Drivers.Domain.Repositories;

public interface IDriverRepository : IBaseRepository<Driver>
{
    Task<Driver?> FindWithRelationsByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Driver>> ListWithRelationsAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, int? excludingId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsByLicenseNumberAsync(string licenseNumber, int? excludingId = null, CancellationToken cancellationToken = default);
}