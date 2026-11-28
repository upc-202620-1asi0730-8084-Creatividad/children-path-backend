using ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;

namespace ChildrenPath.Platform.Analytics.Domain.Model.ReadModels;

public record AnalyticsDashboardSnapshot(
    AnalyticsSummaryData Summary,
    IReadOnlyList<MonitoringMetricData> MonitoringMetrics,
    IReadOnlyList<RoutePerformanceData> Routes,
    IReadOnlyList<InsightData> Insights,
    IReadOnlyList<TrendPointData> Trend,
    IReadOnlyList<Report> Reports,
    IReadOnlyList<ActivityData> Activities,
    DateTimeOffset LastUpdated);

public record AnalyticsSummaryData(
    int TotalTrips,
    int TransportedStudents,
    int OnTimeRate,
    int AverageDelayMinutes,
    double AttendanceRate,
    int ServiceQualityScore,
    int FleetUsage,
    double IncidentRate);

public record MonitoringMetricData(
    string Id, string Label, string Value, string Helper,
    string Icon, string Status, string Trend);

public record RoutePerformanceData(
    string Id, string RouteName, string District, string DriverName, string VehiclePlate,
    int OnTimeRate, int AttendanceRate, int DelayMinutes, int CompletedTrips,
    int IncidentCount, int ServiceScore, string Status);

public record InsightData(
    string Id, string Title, string Description, string Impact, string Source, string Action);

public record TrendPointData(
    string Label, int OnTimeRate, int AttendanceRate, int DelayMinutes);

public record ActivityData(
    string Id, DateTimeOffset Time, string Title, string Description, string Status);