using ChildrenPath.Platform.Drivers.Domain.Model.Entities;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Drivers.Domain.Repositories;

public interface IDriverActivityRepository : IBaseRepository<DriverActivity>
{
    Task<IEnumerable<DriverActivity>> ListRecentAsync(int limit, CancellationToken cancellationToken = default);
}