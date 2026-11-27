namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to cancel an active subscription.
/// </summary>
/// <param name="SubscriptionId">The unique identifier of the subscription to cancel.</param>
public record CancelSubscriptionCommand(int SubscriptionId);