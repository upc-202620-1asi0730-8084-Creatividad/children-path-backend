using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Tracking.Domain.Model.Aggregate;

namespace ChildrenPath.Platform.Tracking.Domain.Repositories;

public interface ITrackingSessionRepository : IBaseRepository<TrackingSession>
{
    Task<IEnumerable<TrackingSession>> FindAllActiveAsync(CancellationToken cancellationToken = default);
}