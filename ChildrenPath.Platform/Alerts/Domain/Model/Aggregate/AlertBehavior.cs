using ChildrenPath.Platform.Alerts.Domain.Model.Entities;
using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;

public partial class Alert 
{
    public AlertStatus Status { get; private set; } = AlertStatus.Active;
    
    public ICollection<Notification> Notifications { get; } = new List<Notification>();

    public void Activate() => Status = AlertStatus.Active;

    public bool Resolve()
    {
        Status = AlertStatus.Resolved;
        return true;
    }

    public void Cancel() => Status = AlertStatus.Cancelled;

    public void AddNotification(Notification notification) => Notifications.Add(notification);
}