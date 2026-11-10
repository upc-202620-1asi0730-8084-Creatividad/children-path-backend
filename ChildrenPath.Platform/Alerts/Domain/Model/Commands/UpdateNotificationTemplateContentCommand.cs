namespace ChildrenPath.Platform.Alerts.Domain.Model.Commands;

/// <summary>
///     Command to update the content of a notification template.
/// </summary>
public record UpdateNotificationTemplateContentCommand(int TemplateId, string NewContent);