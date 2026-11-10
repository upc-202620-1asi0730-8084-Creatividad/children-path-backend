using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Commands;

/// <summary>
///     Command to create a reusable notification template.
/// </summary>
public record CreateNotificationTemplateCommand(
    string Name,
    NotificationChannel Channel,
    string Content);