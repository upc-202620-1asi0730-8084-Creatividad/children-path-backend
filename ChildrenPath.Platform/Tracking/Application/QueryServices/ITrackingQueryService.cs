using ChildrenPath.Platform.Tracking.Domain.Model.Aggregate;
using ChildrenPath.Platform.Tracking.Domain.Model.Queries;

namespace ChildrenPath.Platform.Tracking.Application.QueryServices;

public interface ITrackingQueryService
{
    Task<IEnumerable<TrackingSession>> Handle(GetActiveTrackingSessionsQuery query, CancellationToken cancellationToken);
    
    Task<TrackingSession?> Handle(GetTrackingSessionByIdQuery query, CancellationToken cancellationToken);
}