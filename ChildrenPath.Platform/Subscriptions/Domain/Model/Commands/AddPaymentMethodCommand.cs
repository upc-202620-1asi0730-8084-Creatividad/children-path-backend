using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to attach a new saved payment card to a subscription.
/// </summary>
public record AddPaymentMethodCommand(
    int SubscriptionId,
    CardBrand Brand,
    string CardNumberOrLast4,
    string Holder,
    string ExpiresOn,
    bool MakePrimary = false);