using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to renew an existing subscription for additional months and record the renewal payment.
/// </summary>
public record RenewSubscriptionCommand(
    int SubscriptionId,
    int AdditionalMonths,
    PaymentMethod PaymentMethod);