using ChildrenPath.Platform.Tracking.Application.QueryServices;
using ChildrenPath.Platform.Tracking.Domain.Model.Aggregate;
using ChildrenPath.Platform.Tracking.Domain.Model.Queries;
using ChildrenPath.Platform.Tracking.Domain.Repositories;

namespace ChildrenPath.Platform.Tracking.Application.Internal.QueryServices;

public class TrackingQueryService(ITrackingSessionRepository repository) : ITrackingQueryService
{
    public async Task<IEnumerable<TrackingSession>> Handle(GetActiveTrackingSessionsQuery query, CancellationToken cancellationToken)
    {
        return await repository.FindAllActiveAsync(cancellationToken);
    }
}