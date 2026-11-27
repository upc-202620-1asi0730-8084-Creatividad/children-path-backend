namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to update current resource consumption counters for a subscription.
/// </summary>
public record UpdateSubscriptionUsageCommand(
    int SubscriptionId,
    int VehiclesUsed,
    int StudentsUsed,
    int UsersUsed);