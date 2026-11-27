using ChildrenPath.Platform.Alerts.Domain.Model.Entities;
using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;

public partial class Alert 
{
    private readonly List<Notification> _notifications = new();
    public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();

    public AlertStatus Status { get; private set; } = AlertStatus.Active;
    public bool IsRead { get; private set; } = false;

    public void Activate() => Status = AlertStatus.Active;

    public bool Resolve()
    {
        Status = AlertStatus.Resolved;
        return true;
    }

    public void Cancel() => Status = AlertStatus.Cancelled;

    public void AddNotification(Notification notification) => _notifications.Add(notification);

    public void MarkAsRead() => IsRead = true;
}