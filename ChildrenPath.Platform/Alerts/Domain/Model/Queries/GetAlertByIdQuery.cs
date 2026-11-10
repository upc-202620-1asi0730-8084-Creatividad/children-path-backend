namespace ChildrenPath.Platform.Alerts.Domain.Model.Queries;

/// <summary>
///     Query to get an alert by id.
/// </summary>
/// <param name="AlertId">The id of the alert to get.</param>
public record GetAlertByIdQuery(int AlertId);