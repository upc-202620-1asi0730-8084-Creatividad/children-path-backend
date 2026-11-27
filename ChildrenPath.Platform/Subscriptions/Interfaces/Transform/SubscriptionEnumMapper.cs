using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Interfaces.Transform;

/// <summary>
///     Converts between REST/frontend string keys and Subscriptions domain enums.
/// </summary>
public static class SubscriptionEnumMapper
{
    public static PlanType ParsePlanType(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return (PlanType)(-1);

        return raw.Trim().ToLowerInvariant() switch
        {
            "basic" or "starter" => PlanType.Basic,
            "premium" or "pro" => PlanType.Premium,
            "enterprise" => PlanType.Enterprise,
            _ => (PlanType)(-1)
        };
    }

    public static SubscriptionStatus ParseSubscriptionStatus(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return (SubscriptionStatus)(-1);

        return raw.Trim().ToLowerInvariant() switch
        {
            "active" => SubscriptionStatus.Active,
            "pending" => SubscriptionStatus.Pending,
            "pastdue" or "past-due" or "past_due" => SubscriptionStatus.PastDue,
            "cancelled" or "canceled" => SubscriptionStatus.Cancelled,
            "expired" => SubscriptionStatus.Expired,
            _ => (SubscriptionStatus)(-1)
        };
    }

    public static RenewalMode ParseRenewalMode(string? raw, bool defaultToAssistedWhenEmpty = false)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return defaultToAssistedWhenEmpty ? RenewalMode.Assisted : (RenewalMode)(-1);

        return raw.Trim().ToLowerInvariant() switch
        {
            "automatic" or "auto" => RenewalMode.Automatic,
            "manual" => RenewalMode.Manual,
            "assisted" => RenewalMode.Assisted,
            _ => (RenewalMode)(-1)
        };
    }

    public static PaymentMethod ParsePaymentMethod(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return (PaymentMethod)(-1);

        return raw.Trim().ToLowerInvariant() switch
        {
            "creditcard" or "credit-card" or "credit_card" or "card" => PaymentMethod.CreditCard,
            "debitcard" or "debit-card" or "debit_card" => PaymentMethod.DebitCard,
            "banktransfer" or "bank-transfer" or "bank_transfer" or "transfer" => PaymentMethod.BankTransfer,
            "paypal" => PaymentMethod.PayPal,
            _ => (PaymentMethod)(-1)
        };
    }

    public static CardBrand ParseCardBrand(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return (CardBrand)(-1);

        return raw.Trim().ToLowerInvariant() switch
        {
            "visa" => CardBrand.Visa,
            "mastercard" or "mc" => CardBrand.Mastercard,
            "amex" or "americanexpress" or "american-express" => CardBrand.Amex,
            _ => (CardBrand)(-1)
        };
    }

    public static string ToFrontendSubscriptionStatus(SubscriptionStatus status) => status switch
    {
        SubscriptionStatus.Active => "active",
        SubscriptionStatus.Pending or SubscriptionStatus.PastDue => "pending",
        SubscriptionStatus.Cancelled => "cancelled",
        SubscriptionStatus.Expired => "expired",
        _ => "pending"
    };

    public static string ToFrontendRenewalMode(RenewalMode mode) => mode switch
    {
        RenewalMode.Automatic => "automatic",
        RenewalMode.Manual => "manual",
        RenewalMode.Assisted => "assisted",
        _ => "assisted"
    };

    public static string ToFrontendInvoiceStatus(InvoiceStatus status) => status switch
    {
        InvoiceStatus.Paid => "paid",
        InvoiceStatus.Failed or InvoiceStatus.Voided => "failed",
        _ => "pending"
    };

    public static string ToFrontendCardBrand(CardBrand brand) => brand switch
    {
        CardBrand.Visa => "visa",
        CardBrand.Mastercard => "mastercard",
        CardBrand.Amex => "amex",
        _ => "visa"
    };
}