using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Entities;

/// <summary>
///     Represents a notification sent as part of an alert.
/// </summary>
public class Notification
{
    public Notification() { }

    public Notification(NotificationChannel channel, string recipient)
    {
        Channel = channel;
        Recipient = recipient;
        Status = NotificationStatus.Pending;
        SentAt = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public NotificationChannel Channel { get; private set; }
    public string Recipient { get; private set; } = string.Empty;
    public NotificationStatus Status { get; private set; } = NotificationStatus.Pending;
    public DateTime SentAt { get; private set; } = DateTime.UtcNow;
    public int AlertId { get; private set; } // FK

    // Composición
    public ICollection<DeliveryLog> DeliveryLogs { get; } = new List<DeliveryLog>();

    public bool Send()
    {
        Status = NotificationStatus.Sent;
        return true;
    }

    public void MarkDelivered() => Status = NotificationStatus.Delivered;

    public void AddDeliveryLog(DeliveryLog log) => DeliveryLogs.Add(log);
}