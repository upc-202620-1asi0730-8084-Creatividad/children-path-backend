using ChildrenPath.Platform.Shared.Domain.Model.Events;

namespace ChildrenPath.Platform.Trips.Domain.Model.Events;

public class TripCompletedEvent(int tripId, string code) : IEvent
{
    public int TripId { get; } = tripId;
    public string Code { get; } = code;
}