using ChildrenPath.Platform.Analytics.Application.QueryServices;
using ChildrenPath.Platform.Analytics.Domain.Model.Queries;
using ChildrenPath.Platform.Analytics.Domain.Model.ReadModels;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Analytics.Application.Internal.QueryServices;

public class AnalyticsDashboardQueryService(IReportQueryService reportQueryService)
    : IAnalyticsDashboardQueryService
{
    // TODO: cuando integremos datos reales, inyectar también:
    //   ITripQueryService, IVehicleQueryService, IAlertQueryService,
    //   IStudentQueryService, IAttendanceQueryService.

    public async Task<Result<AnalyticsDashboardSnapshot>> Handle(
        GetAnalyticsDashboardQuery query,
        CancellationToken cancellationToken)
    {
        var reports = await reportQueryService.Handle(query, cancellationToken);

        var snapshot = new AnalyticsDashboardSnapshot(
            Summary: new AnalyticsSummaryData(
                TotalTrips: 184,
                TransportedStudents: 2450,
                OnTimeRate: 91,
                AverageDelayMinutes: 7,
                AttendanceRate: 96.8,
                ServiceQualityScore: 93,
                FleetUsage: 88,
                IncidentRate: 1.8),
            MonitoringMetrics:
            [
                new("m1", "Active monitored routes", "14", "Routes sending operational data",
                    "route", "healthy", "up"),
                new("m2", "Live tracking sessions", "12", "Vehicles currently reporting GPS",
                    "gps_fixed", "healthy", "stable"),
                new("m3", "Attention signals", "3", "Delays, incidents or attendance gaps",
                    "notification_important", "attention", "down")
            ],
            Routes:
            [
                new("rt-001", "Miraflores School Route", "Miraflores", "Carlos Pérez", "KW-204",
                    96, 98, 4, 42, 0, 97, "healthy"),
                new("rt-003", "Surco Pickup Route", "Santiago de Surco", "Luis Torres", "KW-076",
                    78, 92, 18, 36, 2, 82, "risk")
            ],
            Insights:
            [
                new("i1", "Surco route requires optimization",
                    "Delay minutes increased during the last three service days.",
                    "high", "Route and trip analytics", "Review stop sequence")
            ],
            Trend:
            [
                new("Mon", 89, 96, 10),
                new("Tue", 91, 95, 8)
            ],
            Reports: reports.ToList(),
            Activities:
            [
                new("a1", DateTimeOffset.UtcNow.AddHours(-1),
                    "Monitoring snapshot updated", "Live indicators were refreshed.", "healthy")
            ],
            LastUpdated: DateTimeOffset.UtcNow);

        return Result<AnalyticsDashboardSnapshot>.Success(snapshot);
    }
}