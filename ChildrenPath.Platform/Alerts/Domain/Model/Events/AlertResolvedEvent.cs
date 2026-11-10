using ChildrenPath.Platform.Shared.Domain.Model.Events;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Events;

public class AlertResolvedEvent(int alertId, int tripId) : IEvent
{
    public int AlertId { get; } = alertId;
    public int TripId { get; } = tripId;
}