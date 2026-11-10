namespace ChildrenPath.Platform.Alerts.Domain.Model.Entities;

/// <summary>
///     Represents a delivery log entry for a notification.
/// </summary>
public class DeliveryLog
{
    public DeliveryLog() { }

    public DeliveryLog(string response)
    {
        Response = response;
        LoggedAt = DateTime.UtcNow;
    }

    public int Id { get; set; }
    public string Response { get; set; } = string.Empty;
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;
}