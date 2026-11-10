namespace ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

/// <summary>
///     Represents the delivery status of a notification.
/// </summary>
public enum NotificationStatus
{
    Pending,
    Sent,
    Delivered,
    Failed
}