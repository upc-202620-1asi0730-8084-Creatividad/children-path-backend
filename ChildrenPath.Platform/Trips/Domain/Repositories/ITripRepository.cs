using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Trips.Domain.Model.Aggregate;
using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Trips.Domain.Repositories;

public interface ITripRepository : IBaseRepository<Trip>
{
    Task<Trip?> FindByCodeAsync(string code, CancellationToken ct = default);
    Task<IEnumerable<Trip>> FindByStatusAsync(TripStatus status, CancellationToken ct = default);
    Task<IEnumerable<Trip>> FindByShiftAsync(TripShift shift, CancellationToken ct = default);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken ct = default);
}