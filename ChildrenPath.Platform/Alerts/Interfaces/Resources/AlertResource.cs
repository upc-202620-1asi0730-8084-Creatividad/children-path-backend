namespace ChildrenPath.Platform.Alerts.Interfaces.Resources;

public record AlertResource(
    int Id,
    int TripId,
    string Type,
    string Priority,
    string Message,
    string Status,
    DateTime CreatedAt,
    List<NotificationResource> Notifications
);

public record NotificationResource(
    int Id,
    string Channel,
    string Recipient,
    string Status,
    DateTime SentAt
);