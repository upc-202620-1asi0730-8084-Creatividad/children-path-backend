namespace ChildrenPath.Platform.Alerts.Domain.Model.ReadModels;

public record AlertDashboardSnapshot(
    AlertDashboardSummaryData Summary,
    IReadOnlyList<AlertDashboardItemData> Items,
    IReadOnlyList<AlertDashboardActivityData> Activities);

public record AlertDashboardSummaryData(
    int Total,
    int Unread,
    int CriticalAlerts,
    double DeliveryRate,
    int Acknowledged);

public record AlertDashboardItemData(
    int AlertId,
    string Category,
    string Type,
    string Priority,
    string Status,
    string Title,
    string Message,
    DateTimeOffset CreatedAt,
    string SourceBc,
    string RecipientRole,
    string RouteName,
    string VehiclePlate,
    string ActionLabel);

public record AlertDashboardActivityData(
    string Id,
    DateTimeOffset Time,
    string Title,
    string Description,
    string Status);