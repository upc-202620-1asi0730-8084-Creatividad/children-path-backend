using ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;
using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Alerts.Interfaces.Resources;

namespace ChildrenPath.Platform.Alerts.Interfaces.Transform;

public static class AlertDashboardAssembler
{
    public static AlertDashboardResource ToDashboardResource(IEnumerable<Alert> alerts)
    {
        var alertList = alerts.ToList();

        var items = alertList.Select(a => new AlertDashboardItemResource(
            Id: $"al-{a.Id:D3}",
            Category: a.Type == AlertType.Delay || a.Type == AlertType.Emergency ? "alert" : "notification",
            Type: a.Type.ToString().ToLower(),
            Priority: a.Priority.ToString().ToLower(),
            Status: a.Status == AlertStatus.Resolved ? "acknowledged" : "new", // Mapeo de estados
            Title: a.Type == AlertType.Delay ? "Route delay detected" : "Incident report",
            Message: a.Message,
            Time: a.CreatedAt.ToString("h:mm tt"),
            Date: a.CreatedAt.Date == DateTime.UtcNow.Date ? "Today" : a.CreatedAt.ToString("MMM dd"),
            SourceBc: "Trip Management",
            RecipientRole: "Company Admin",
            RouteName: "San Isidro Morning Route",
            VehiclePlate: "KW-118",
            ActionLabel: "Review alert"
        )).ToList();

        var summary = new AlertDashboardSummaryResource(
            Total: items.Count,
            Unread: items.Count(i => i.Status == "new"),
            CriticalAlerts: alertList.Count(a => a.Priority == AlertPriority.Critical),
            DeliveryRate: 98.4,
            Acknowledged: items.Count(i => i.Status == "acknowledged")
        );

        var activities = new List<AlertDashboardActivityResource>
        {
            new("act-001", "Just now", "System Active", "Connected to backend.", "completed")
        };

        return new AlertDashboardResource(summary, items, activities);
    }
}