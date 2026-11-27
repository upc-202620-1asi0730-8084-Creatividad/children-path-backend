using ChildrenPath.Platform.Alerts.Application.QueryServices;
using ChildrenPath.Platform.Alerts.Domain.Model.Queries;
using ChildrenPath.Platform.Alerts.Domain.Model.ReadModels;
using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Alerts.Application.Internal.QueryServices;

public class AlertDashboardQueryService(IAlertQueryService alertQueryService)
    : IAlertDashboardQueryService
{

    public async Task<Result<AlertDashboardSnapshot>> Handle(
        GetAlertDashboardQuery query,
        CancellationToken cancellationToken)
    {
        var alerts = (await alertQueryService
            .Handle(new GetAllActiveAlertsQuery(), cancellationToken))
            .ToList();

        var items = alerts.Select(a => new AlertDashboardItemData(
            AlertId: a.Id,
            Category: a.Type is AlertType.Delay or AlertType.Emergency ? "alert" : "notification",
            Type: a.Type.ToString().ToLowerInvariant(),
            Priority: a.Priority.ToString().ToLowerInvariant(),
            Status: a.Status == AlertStatus.Resolved ? "acknowledged" : "new",
            Title: a.Type == AlertType.Delay ? "Route delay detected" : "Incident report",
            Message: a.Message,
            CreatedAt: a.CreatedAt ?? DateTimeOffset.UtcNow,
            // --- TODO: placeholders hasta integrar Trips/Fleet/Iam ---
            SourceBc: "Trip Management",
            RecipientRole: "Company Admin",
            RouteName: "San Isidro Morning Route",
            VehiclePlate: "KW-118",
            ActionLabel: "Review alert"
        )).ToList();

        var summary = new AlertDashboardSummaryData(
            Total: items.Count,
            Unread: items.Count(i => i.Status == "new"),
            CriticalAlerts: alerts.Count(a => a.Priority == AlertPriority.Critical),
            DeliveryRate: 98.4, // TODO: calcular desde Notification.Status
            Acknowledged: items.Count(i => i.Status == "acknowledged"));

        var activities = new List<AlertDashboardActivityData>
        {
            new("act-001", DateTimeOffset.UtcNow, "System Active", "Connected to backend.", "completed")
        };

        return Result<AlertDashboardSnapshot>.Success(
            new AlertDashboardSnapshot(summary, items, activities));
    }
}