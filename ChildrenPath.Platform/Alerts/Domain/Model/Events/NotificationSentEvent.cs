using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain.Model.Events;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Events;

public class NotificationSentEvent(int alertId, int notificationId, NotificationChannel channel, string recipient) : IEvent
{
    public int AlertId { get; } = alertId;
    public int NotificationId { get; } = notificationId;
    public NotificationChannel Channel { get; } = channel;
    public string Recipient { get; } = recipient;
}