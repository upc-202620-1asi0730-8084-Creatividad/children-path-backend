using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Queries;

/// <summary>
///     Query to get all alerts with a specific status.
/// </summary>
/// <param name="Status">The status to filter by.</param>
public record GetAlertsByStatusQuery(AlertStatus Status);