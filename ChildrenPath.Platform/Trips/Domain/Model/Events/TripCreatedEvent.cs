using ChildrenPath.Platform.Shared.Domain.Model.Events;
using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Trips.Domain.Model.Events;

public class TripCreatedEvent(int tripId, string code, string routeName, TripShift shift) : IEvent
{
    public int TripId { get; } = tripId;
    public string Code { get; } = code;
    public string RouteName { get; } = routeName;
    public TripShift Shift { get; } = shift;
}