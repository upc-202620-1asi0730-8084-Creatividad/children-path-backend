using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Entities;

/// <summary>
///     Represents a stored payment card associated with a subscription account.
/// </summary>
public class SavedPaymentMethod
{
    public int Id { get; private set; }
    public int SubscriptionId { get; private set; }
    public CardBrand Brand { get; private set; } = CardBrand.Visa;
    public string Last4 { get; private set; } = string.Empty;
    public string Holder { get; private set; } = string.Empty;
    public string ExpiresOn { get; private set; } = string.Empty;
    public bool IsPrimary { get; private set; }

    /// <summary>
    ///     Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected SavedPaymentMethod()
    {
    }

    public SavedPaymentMethod(
        CardBrand brand,
        string last4,
        string holder,
        string expiresOn,
        bool isPrimary = false)
    {
        Brand = brand;
        Last4 = NormalizeLast4(last4);
        Holder = holder.Trim();
        ExpiresOn = expiresOn.Trim();
        IsPrimary = isPrimary;
    }

    public void SetAsPrimary() => IsPrimary = true;

    public void UnsetPrimary() => IsPrimary = false;

    public string ToDisplaySummary()
    {
        var prefix = Brand switch
        {
            CardBrand.Visa => "Visa",
            CardBrand.Mastercard => "MC",
            CardBrand.Amex => "Amex",
            _ => Brand.ToString()
        };

        return $"{prefix} ••{Last4}";
    }

    private static string NormalizeLast4(string raw)
    {
        var digits = new string(raw.Where(char.IsDigit).ToArray());
        return digits.Length >= 4 ? digits[^4..] : digits.PadLeft(4, '0');
    }
}