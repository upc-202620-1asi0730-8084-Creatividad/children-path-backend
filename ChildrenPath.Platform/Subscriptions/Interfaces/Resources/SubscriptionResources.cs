namespace ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

/// <summary>
///     Represents a payment transaction associated with an invoice.
/// </summary>
public record PaymentResource(
    int Id,
    string TransactionId,
    decimal Amount,
    string Currency,
    string Method,
    DateTime PaidAt,
    bool IsRefunded);

/// <summary>
///     Represents a billing invoice associated with a subscription.
/// </summary>
public record InvoiceResource(
    int Id,
    string InvoiceNumber,
    string Description,
    decimal Amount,
    string Currency,
    DateTime IssuedAt,
    string Status,
    string PaymentMethodSummary,
    string InvoiceUrl,
    IEnumerable<PaymentResource> Payments);

/// <summary>
///     Represents a saved payment card associated with a subscription.
/// </summary>
public record SavedPaymentMethodResource(
    int Id,
    string Brand,
    string Last4,
    string Holder,
    string ExpiresOn,
    bool IsPrimary);

/// <summary>
///     Represents a full subscription aggregate returned by the standard REST API.
/// </summary>
public record SubscriptionResource(
    int Id,
    int UserId,
    int PlanId,
    string PlanCode,
    string PlanName,
    decimal? PlanPrice,
    string Currency,
    DateTime StartDate,
    DateTime EndDate,
    string Status,
    string ContractOwner,
    string BillingEmail,
    string RenewalMode,
    string SupportContact,
    string PlanScope,
    int VehiclesUsed,
    int StudentsUsed,
    int UsersUsed,
    IEnumerable<InvoiceResource> Invoices,
    IEnumerable<SavedPaymentMethodResource> PaymentMethods);

/// <summary>
///     Request payload for renewing an existing subscription.
/// </summary>
public record RenewSubscriptionResource(
    int AdditionalMonths,
    string PaymentMethod);

/// <summary>
///     Request payload for switching an existing subscription to another plan.
/// </summary>
public record ChangeSubscriptionPlanResource(
    int NewPlanId,
    int DurationInMonths,
    string PaymentMethod);

/// <summary>
///     Request payload for updating resource usage counters on a subscription.
/// </summary>
public record UpdateSubscriptionUsageResource(
    int VehiclesUsed,
    int StudentsUsed,
    int UsersUsed);

/// <summary>
///     Request payload for paying an unpaid invoice.
/// </summary>
public record PayInvoiceResource(
    string Method,
    string? MethodSummary = null);

/// <summary>
///     Request payload for adding a saved payment card to a subscription.
/// </summary>
public record AddPaymentMethodResource(
    string Brand,
    string CardNumber,
    string Holder,
    string ExpiresOn,
    bool MakePrimary = false);