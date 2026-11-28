namespace ChildrenPath.Platform.Tracking.Interfaces.Resources;

public record TrackingDashboardResource(
    TrackingDashboardSummaryResource Summary,
    IEnumerable<TrackingSessionResource> Vehicles);

public record TrackingDashboardSummaryResource(
    int ActiveUnits,
    double AverageSpeed,
    int ActiveAlerts);