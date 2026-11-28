using ChildrenPath.Platform.Alerts.Domain.Model.ReadModels;
using ChildrenPath.Platform.Alerts.Interfaces.Resources;

namespace ChildrenPath.Platform.Alerts.Interfaces.Transform;

public static class AlertDashboardAssembler
{
    public static AlertDashboardResource ToDashboardResource(AlertDashboardSnapshot snapshot)
    {
        var now = DateTimeOffset.UtcNow;

        var items = snapshot.Items.Select(i => new AlertDashboardItemResource(
            Id: $"al-{i.AlertId:D3}",
            Category: i.Category,
            Type: i.Type,
            Priority: i.Priority,
            Status: i.Status,
            Title: i.Title,
            Message: i.Message,
            Time: i.CreatedAt.ToString("h:mm tt"),
            Date: i.CreatedAt.Date == now.Date ? "Today" : i.CreatedAt.ToString("MMM dd"),
            SourceBc: i.SourceBc,
            RecipientRole: i.RecipientRole,
            RouteName: i.RouteName,
            VehiclePlate: i.VehiclePlate,
            ActionLabel: i.ActionLabel
        )).ToList();

        var summary = new AlertDashboardSummaryResource(
            Total: snapshot.Summary.Total,
            Unread: snapshot.Summary.Unread,
            CriticalAlerts: snapshot.Summary.CriticalAlerts,
            DeliveryRate: snapshot.Summary.DeliveryRate,
            Acknowledged: snapshot.Summary.Acknowledged);

        var activities = snapshot.Activities.Select(a => new AlertDashboardActivityResource(
            a.Id, FormatRelativeTime(a.Time, now), a.Title, a.Description, a.Status)).ToList();

        return new AlertDashboardResource(summary, items, activities);
    }

    private static string FormatRelativeTime(DateTimeOffset time, DateTimeOffset now)
    {
        var diff = now - time;
        if (diff.TotalMinutes < 1) return "Just now";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} min ago";
        if (diff.TotalHours   < 24) return $"{(int)diff.TotalHours} h ago";
        return time.ToString("MMM dd, h:mm tt");
    }
}