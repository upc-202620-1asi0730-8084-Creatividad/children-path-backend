using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Entities;

/// <summary>
///     Represents a payment transaction linked to a billing invoice.
/// </summary>
public class Payment
{
    public int Id { get; private set; }
    public string TransactionId { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "USD";
    public PaymentMethod Method { get; private set; }
    public DateTime PaidAt { get; private set; } = DateTime.UtcNow;
    public bool IsRefunded { get; private set; }
    public int InvoiceId { get; private set; }

    /// <summary>
    ///     Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Payment()
    {
    }

    public Payment(
        string transactionId,
        decimal amount,
        PaymentMethod method,
        string currency = "USD",
        DateTime? paidAt = null)
    {
        TransactionId = string.IsNullOrWhiteSpace(transactionId)
            ? $"TXN-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}"
            : transactionId.Trim();
        Amount = Math.Max(amount, 0m);
        Currency = string.IsNullOrWhiteSpace(currency) ? "USD" : currency.Trim().ToUpperInvariant();
        Method = method;
        PaidAt = paidAt ?? DateTime.UtcNow;
        IsRefunded = false;
    }

    public bool Process() => !IsRefunded && Amount >= 0m;

    public void Refund()
    {
        IsRefunded = true;
    }
}