using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Tracking.Domain.Model.Queries;
using ChildrenPath.Platform.Tracking.Domain.Model.ReadModels;

namespace ChildrenPath.Platform.Tracking.Application.QueryServices;

public interface ITrackingDashboardQueryService
{
    Task<Result<TrackingDashboardSnapshot>> Handle(
        GetTrackingDashboardQuery query, 
        CancellationToken cancellationToken);
}