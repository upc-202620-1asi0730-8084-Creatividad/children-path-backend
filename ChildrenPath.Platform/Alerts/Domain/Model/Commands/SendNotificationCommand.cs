using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Commands;

/// <summary>
///     Command to send a notification as part of an alert.
/// </summary>
/// <param name="AlertId">The ID of the alert that fires the notification.</param>
/// <param name="Channel">The delivery channel.</param>
/// <param name="Recipient">The recipient identifier (email, phone, device token...).</param>
public record SendNotificationCommand(
    int AlertId,
    NotificationChannel Channel,
    string Recipient);