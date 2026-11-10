using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Entities;

/// <summary>
///     Represents a configurable rule that triggers alerts.
/// </summary>
public class AlertRule
{
    public int Id { get; set; }
    public AlertType Type { get; set; }
    public bool Enabled { get; set; } = true;
    public int Threshold { get; set; }

    public void Enable() => Enabled = true;
    public void Disable() => Enabled = false;
}