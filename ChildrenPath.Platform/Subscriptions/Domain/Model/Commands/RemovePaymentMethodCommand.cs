namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to remove a saved payment card from a subscription.
/// </summary>
public record RemovePaymentMethodCommand(int SubscriptionId, int PaymentMethodId);