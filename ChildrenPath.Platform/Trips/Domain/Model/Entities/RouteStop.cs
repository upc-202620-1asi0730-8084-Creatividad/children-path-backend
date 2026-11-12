using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Trips.Domain.Model.Entities;

/// <summary>
///     Represents a scheduled pickup or drop-off stop within a trip.
/// </summary>
public class RouteStop
{
    public RouteStop() { }

    public RouteStop(string name, string address, int order, string scheduledTime)
    {
        Name = name;
        Address = address;
        Order = order;
        ScheduledTime = scheduledTime;
        Status = StopStatus.Pending;
    }

    public int Id { get; set; }
    public int TripId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public int Order { get; private set; }
    public string ScheduledTime { get; private set; } = string.Empty;
    public StopStatus Status { get; private set; } = StopStatus.Pending;

    public void MarkAsReached() => Status = StopStatus.Reached;
    public void Skip() => Status = StopStatus.Skipped;
}