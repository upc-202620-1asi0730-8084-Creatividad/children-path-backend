using ChildrenPath.Platform.Analytics.Domain.Model.ReadModels;
using ChildrenPath.Platform.Analytics.Interfaces.Resources;

namespace ChildrenPath.Platform.Analytics.Interfaces.Transform;

public static class AnalyticsDashboardResourceFromSnapshotAssembler
{
    public static AnalyticsDashboardResource ToResourceFromSnapshot(AnalyticsDashboardSnapshot s)
    {
        return new AnalyticsDashboardResource(
            Summary: new AnalyticsSummaryResource(
                s.Summary.TotalTrips, s.Summary.TransportedStudents, s.Summary.OnTimeRate,
                s.Summary.AverageDelayMinutes, s.Summary.AttendanceRate,
                s.Summary.ServiceQualityScore, s.Summary.FleetUsage, s.Summary.IncidentRate),
            MonitoringMetrics: s.MonitoringMetrics.Select(m =>
                new MonitoringMetricResource(m.Id, m.Label, m.Value, m.Helper, m.Icon, m.Status, m.Trend)).ToList(),
            Routes: s.Routes.Select(r =>
                new RoutePerformanceResource(r.Id, r.RouteName, r.District, r.DriverName, r.VehiclePlate,
                    r.OnTimeRate, r.AttendanceRate, r.DelayMinutes, r.CompletedTrips,
                    r.IncidentCount, r.ServiceScore, r.Status)).ToList(),
            Insights: s.Insights.Select(i =>
                new InsightResource(i.Id, i.Title, i.Description, i.Impact, i.Source, i.Action)).ToList(),
            Trend: s.Trend.Select(t =>
                new TrendPointResource(t.Label, t.OnTimeRate, t.AttendanceRate, t.DelayMinutes)).ToList(),
            Reports: s.Reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity).ToList(),
            Activities: s.Activities.Select(a =>
                new ActivityResource(a.Id, a.Time, a.Title, a.Description, a.Status)).ToList(),
            LastUpdated: s.LastUpdated);
    }
}