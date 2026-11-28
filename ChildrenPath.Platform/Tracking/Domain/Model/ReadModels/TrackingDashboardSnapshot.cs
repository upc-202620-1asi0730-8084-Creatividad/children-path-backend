using ChildrenPath.Platform.Tracking.Domain.Model.Aggregate;

namespace ChildrenPath.Platform.Tracking.Domain.Model.ReadModels;

public record TrackingDashboardSnapshot(
    TrackingDashboardSummaryData Summary,
    IReadOnlyList<TrackingSession> Sessions);

public record TrackingDashboardSummaryData(
    int ActiveUnits,
    double AverageSpeed,
    int ActiveAlerts);