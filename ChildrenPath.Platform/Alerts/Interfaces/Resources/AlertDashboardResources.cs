namespace ChildrenPath.Platform.Alerts.Interfaces.Resources;

public record AlertDashboardResource(
    AlertDashboardSummaryResource Summary,
    IEnumerable<AlertDashboardItemResource> Items,
    IEnumerable<AlertDashboardActivityResource> Activities
);

public record AlertDashboardSummaryResource(
    int Total,
    int Unread,
    int CriticalAlerts,
    double DeliveryRate,
    int Acknowledged
);

public record AlertDashboardItemResource(
    string Id,
    string Category,
    string Type,
    string Priority,
    string Status,
    string Title,
    string Message,
    string Time,
    string Date,
    string SourceBc,
    string RecipientRole,
    string RouteName,
    string VehiclePlate,
    string ActionLabel
);

public record AlertDashboardActivityResource(
    string Id,
    string Time,
    string Title,
    string Description,
    string Status
);