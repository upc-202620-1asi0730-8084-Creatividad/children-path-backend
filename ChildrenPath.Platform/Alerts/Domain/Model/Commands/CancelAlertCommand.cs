namespace ChildrenPath.Platform.Alerts.Domain.Model.Commands;

/// <summary>
///     Command to cancel an existing alert.
/// </summary>
/// <param name="AlertId">The ID of the alert to cancel.</param>
public record CancelAlertCommand(int AlertId);