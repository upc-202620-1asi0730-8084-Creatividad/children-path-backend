namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to mark a saved payment card as the primary billing method.
/// </summary>
public record SetPrimaryPaymentMethodCommand(int SubscriptionId, int PaymentMethodId);