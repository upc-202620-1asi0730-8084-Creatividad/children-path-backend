namespace ChildrenPath.Platform.Alerts.Interfaces.Resources;

public record CreateAlertResource(
    int TripId,
    string Type,
    string Priority,
    string Message,
    string Recipient,
    string Channel
);