namespace ChildrenPath.Platform.Alerts.Domain.Model.Commands;

/// <summary>
///     Command to resolve an existing alert.
/// </summary>
/// <param name="AlertId">The ID of the alert to resolve.</param>
public record ResolveAlertCommand(int AlertId);