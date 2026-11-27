using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Entities;

/// <summary>
///     Represents a billing invoice issued under a subscription.
/// </summary>
public class Invoice
{
    public int Id { get; private set; }
    public string InvoiceNumber { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "USD";
    public DateTime IssuedAt { get; private set; } = DateTime.UtcNow;
    public InvoiceStatus Status { get; private set; } = InvoiceStatus.Pending;
    public string PaymentMethodSummary { get; private set; } = string.Empty;
    public string InvoiceUrl { get; private set; } = "#";
    public int SubscriptionId { get; private set; }

    public List<Payment> Payments { get; private set; } = [];

    /// <summary>
    ///     Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Invoice()
    {
    }

    public Invoice(
        string invoiceNumber,
        string description,
        decimal amount,
        string currency = "USD",
        InvoiceStatus status = InvoiceStatus.Pending,
        string paymentMethodSummary = "",
        string invoiceUrl = "#",
        DateTime? issuedAt = null)
    {
        InvoiceNumber = invoiceNumber.Trim();
        Description = description.Trim();
        Amount = Math.Max(amount, 0m);
        Currency = string.IsNullOrWhiteSpace(currency) ? "USD" : currency.Trim().ToUpperInvariant();
        Status = status;
        PaymentMethodSummary = paymentMethodSummary?.Trim() ?? string.Empty;
        InvoiceUrl = string.IsNullOrWhiteSpace(invoiceUrl) ? "#" : invoiceUrl.Trim();
        IssuedAt = issuedAt ?? DateTime.UtcNow;
    }

    public Payment RegisterPayment(PaymentMethod method, string? transactionId = null, string? methodSummary = null)
    {
        var payment = new Payment(
            transactionId ?? $"TXN-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}",
            Amount,
            method,
            Currency);

        Payments.Add(payment);

        if (!string.IsNullOrWhiteSpace(methodSummary))
            PaymentMethodSummary = methodSummary.Trim();

        if (payment.Process())
            MarkAsPaid();

        return payment;
    }

    public void MarkAsPaid() => Status = InvoiceStatus.Paid;

    public void MarkAsFailed() => Status = InvoiceStatus.Failed;

    public void VoidInvoice() => Status = InvoiceStatus.Voided;
}