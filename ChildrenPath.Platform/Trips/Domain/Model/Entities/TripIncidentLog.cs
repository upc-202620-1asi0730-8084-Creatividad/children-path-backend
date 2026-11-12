namespace ChildrenPath.Platform.Trips.Domain.Model.Entities;

/// <summary>
///     Represents an operational log entry recorded during a trip.
/// </summary>
public class TripIncidentLog
{
    public TripIncidentLog() { }

    public TripIncidentLog(string description)
    {
        Description = description;
        LoggedAt = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public int TripId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
}