namespace ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

/// <summary>
///     Represents the type of alert in the ChildrenPath platform.
/// </summary>
public enum AlertType
{
    Delay,
    RouteDeviation,
    SpeedViolation,
    Emergency,
    Absence
}