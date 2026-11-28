using ChildrenPath.Platform.Dashboard.Application.QueryServices;
using ChildrenPath.Platform.Dashboard.Domain.Model.Errors;
using ChildrenPath.Platform.Dashboard.Domain.Model.Queries;
using ChildrenPath.Platform.Dashboard.Domain.Model.ReadModels;
using ChildrenPath.Platform.Dashboard.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Dashboard.Application.Internal.QueryServices;

public class DashboardQueryService : IDashboardQueryService
{

    public Task<Result<DashboardSnapshot>> Handle(GetDashboardForRoleQuery query, CancellationToken cancellationToken)
    {
        DashboardSnapshot? snapshot = query.Type switch
        {
            DashboardType.Company       => BuildCompanySnapshot(),
            DashboardType.Parent        => BuildParentSnapshot(),
            DashboardType.Driver        => BuildDriverSnapshot(),
            DashboardType.Administrator => BuildAdminSnapshot(),
            _                           => null
        };

        return snapshot is null
            ? Task.FromResult(Result<DashboardSnapshot>.Failure(DashboardErrors.InvalidRole))
            : Task.FromResult(Result<DashboardSnapshot>.Success(snapshot));
    }

    // ---- TEMPORAL: datos mock hasta integrar con Trips/Fleet/Alerts/Students ----
    private static DashboardSnapshot BuildCompanySnapshot() => new(
        Type: DashboardType.Company,
        HeroValue: "96.8%",
        Metrics:
        [
            new("trips",    "route",                "12",  "dashboardPage.metrics.activeTrips",     "dashboardPage.helpers.activeTrips", "+3",  "up"),
            new("students", "school",               "184", "dashboardPage.metrics.studentsOnBoard", "dashboardPage.helpers.students",    "+24", "up"),
            new("alerts",   "notifications_active", "4",   "dashboardPage.metrics.openAlerts",      "dashboardPage.helpers.alerts",      "-2",  "down")
        ],
        Vehicles:
        [
            new("kw-204", "KW-204", "Carlos Pérez", -12.0464, -77.0428, "on-route", "Miraflores School Route",        18,  7,  "2 min ago"),
            new("kw-118", "KW-118", "María Gómez",  -12.0931, -77.0465, "delayed",  "San Isidro Morning Route",       21, 12,  "1 min ago")
        ],
        Alerts:
        [
            new(1, "delay", "high", "Route delay detected",
                "Vehicle KW-118 is 8 minutes behind schedule.", "Today, 7:45 AM", false, "KW-118")
        ],
        OperationItems:
        [
            new("attendance", "Attendance reliability", "96.8%", "184 students confirmed", 96.8, "fact_check")
        ],
        Activities:
        [
            new("a1", "7:10 AM", "Morning dispatch completed", "12 routes started with assigned drivers.", "completed")
        ]);

    private static DashboardSnapshot BuildParentSnapshot() => new(
        Type: DashboardType.Parent,
        HeroValue: "1",
        Metrics: [], Vehicles: [], Alerts: [], OperationItems: [], Activities: []);

    private static DashboardSnapshot BuildDriverSnapshot() =>
        BuildParentSnapshot() with { Type = DashboardType.Driver };

    private static DashboardSnapshot BuildAdminSnapshot() =>
        BuildCompanySnapshot() with { Type = DashboardType.Administrator };
}