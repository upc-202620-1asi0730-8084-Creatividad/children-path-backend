namespace ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

/// <summary>
///     Represents the billing status of an invoice.
/// </summary>
public enum InvoiceStatus
{
    Draft = 0,
    Issued = 1,
    Pending = 2,
    Paid = 3,
    Failed = 4,
    Voided = 5
}