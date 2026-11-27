namespace ChildrenPath.Platform.Analytics.Interfaces.Resources;

public record AnalyticsDashboardResource(
    AnalyticsSummaryResource Summary,
    IReadOnlyList<MonitoringMetricResource> MonitoringMetrics,
    IReadOnlyList<RoutePerformanceResource> Routes,
    IReadOnlyList<InsightResource> Insights,
    IReadOnlyList<TrendPointResource> Trend,
    IReadOnlyList<ReportResource> Reports,
    IReadOnlyList<ActivityResource> Activities,
    DateTimeOffset LastUpdated);

public record AnalyticsSummaryResource(
    int TotalTrips, int TransportedStudents, int OnTimeRate, int AverageDelayMinutes,
    double AttendanceRate, int ServiceQualityScore, int FleetUsage, double IncidentRate);

public record MonitoringMetricResource(
    string Id, string Label, string Value, string Helper,
    string Icon, string Status, string Trend);

public record RoutePerformanceResource(
    string Id, string RouteName, string District, string DriverName, string VehiclePlate,
    int OnTimeRate, int AttendanceRate, int DelayMinutes, int CompletedTrips,
    int IncidentCount, int ServiceScore, string Status);

public record InsightResource(
    string Id, string Title, string Description, string Impact, string Source, string Action);

public record TrendPointResource(
    string Label, int OnTimeRate, int AttendanceRate, int DelayMinutes);

public record ActivityResource(
    string Id, DateTimeOffset Time, string Title, string Description, string Status);