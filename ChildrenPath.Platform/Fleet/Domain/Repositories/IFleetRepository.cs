using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Fleet.Domain.Repositories;

public interface IFleetRepository : IBaseRepository<TransportFleet>
{
    Task<IEnumerable<TransportFleet>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}
