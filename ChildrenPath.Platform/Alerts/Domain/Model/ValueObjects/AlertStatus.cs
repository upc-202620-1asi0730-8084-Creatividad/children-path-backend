namespace ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

/// <summary>
///     Represents the lifecycle status of an alert.
/// </summary>
public enum AlertStatus
{
    Active,
    Resolved,
    Cancelled
}