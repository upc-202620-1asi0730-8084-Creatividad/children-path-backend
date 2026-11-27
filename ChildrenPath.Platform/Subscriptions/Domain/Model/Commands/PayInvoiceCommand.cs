using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to settle an unpaid invoice belonging to a subscription.
/// </summary>
public record PayInvoiceCommand(
    int SubscriptionId,
    int InvoiceId,
    PaymentMethod Method,
    string? MethodSummary = null);