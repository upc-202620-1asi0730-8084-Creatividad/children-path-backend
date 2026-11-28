using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Tracking.Application.QueryServices;
using ChildrenPath.Platform.Tracking.Domain.Model.Queries;
using ChildrenPath.Platform.Tracking.Domain.Model.ReadModels;

namespace ChildrenPath.Platform.Tracking.Application.Internal.QueryServices;

public class TrackingDashboardQueryService(ITrackingQueryService trackingQueryService) 
    : ITrackingDashboardQueryService
{
    public async Task<Result<TrackingDashboardSnapshot>> Handle(
        GetTrackingDashboardQuery query, 
        CancellationToken cancellationToken)
    {
        var activeSessions = (await trackingQueryService.Handle(new GetActiveTrackingSessionsQuery(), cancellationToken)).ToList();
        var activeUnits = activeSessions.Count;
        
        var averageSpeed = activeUnits > 0 
            ? activeSessions.Average(s => s.Locations.LastOrDefault()?.Speed ?? 0.0) 
            : 0.0;

        var activeAlerts = 0; 

        var summary = new TrackingDashboardSummaryData(activeUnits, averageSpeed, activeAlerts);

        return Result<TrackingDashboardSnapshot>.Success(
            new TrackingDashboardSnapshot(summary, activeSessions));
    }
}