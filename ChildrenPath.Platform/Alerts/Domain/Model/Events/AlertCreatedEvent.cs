using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain.Model.Events;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Events;

public class AlertCreatedEvent(int alertId, int tripId, AlertType type, AlertPriority priority) : IEvent
{
    public int AlertId { get; } = alertId;
    public int TripId { get; } = tripId;
    public AlertType Type { get; } = type;
    public AlertPriority Priority { get; } = priority;
}