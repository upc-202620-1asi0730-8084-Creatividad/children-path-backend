namespace ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
///     Represents the payment instrument type used to settle an invoice.
/// </summary>
public enum PaymentMethod
{
    CreditCard = 0,
    DebitCard = 1,
    BankTransfer = 2,
    PayPal = 3
}