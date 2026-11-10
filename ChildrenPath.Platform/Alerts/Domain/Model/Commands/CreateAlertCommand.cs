using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Commands;

/// <summary>
///     Command to create an alert.
/// </summary>
/// <param name="TripId">The ID of the trip the alert belongs to.</param>
/// <param name="Type">The type of the alert.</param>
/// <param name="Priority">The priority of the alert.</param>
/// <param name="Message">The human-readable message.</param>
public record CreateAlertCommand(
    int TripId,
    AlertType Type,
    AlertPriority Priority,
    string Message);