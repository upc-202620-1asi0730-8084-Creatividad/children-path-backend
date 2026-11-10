namespace ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

/// <summary>
///     Represents the delivery channel of a notification.
/// </summary>
public enum NotificationChannel
{
    Push,
    Email,
    SMS,
    WhatsApp
}