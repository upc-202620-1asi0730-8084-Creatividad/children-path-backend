using ChildrenPath.Platform.Trips.Domain.Model.Entities;
using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Trips.Domain.Model.Aggregate;

public partial class Trip
{
    public TripStatus Status { get; private set; } = TripStatus.Scheduled;

    public ICollection<RouteStop> Stops { get; } = new List<RouteStop>();
    public ICollection<TripIncidentLog> IncidentLogs { get; } = new List<TripIncidentLog>();

    public void Start() => Status = TripStatus.InProgress;

    public bool Complete()
    {
        Status = TripStatus.Completed;
        return true;
    }

    public void Cancel() => Status = TripStatus.Cancelled;

    public void MarkDelayed() => Status = TripStatus.Delayed;

    public void AddStop(RouteStop stop) => Stops.Add(stop);

    public void AddIncidentLog(TripIncidentLog log) => IncidentLogs.Add(log);

    public void UpdateStudentsCount(int studentsCount)
    {
        if (studentsCount >= 0 && studentsCount <= Capacity)
            Students = studentsCount;
    }
}