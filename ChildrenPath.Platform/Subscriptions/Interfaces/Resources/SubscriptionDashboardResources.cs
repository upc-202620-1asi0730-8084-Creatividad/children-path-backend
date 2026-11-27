namespace ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

/// <summary>
///     Current subscription contract payload consumed by GET /api/v1/currentSubscription.
/// </summary>
public record CurrentSubscriptionDashboardResource(
    string Id,
    string PlanName,
    string Status,
    decimal Price,
    string Currency,
    string Interval,
    string RenewsOn,
    string StartedOn,
    string ContractOwner,
    string BillingEmail,
    string RenewalMode,
    string SupportContact,
    string PlanScope,
    int? VehicleLimit,
    int? StudentLimit,
    int? UserLimit,
    int VehiclesUsed,
    int StudentsUsed,
    int UsersUsed);

/// <summary>
///     Plan card payload consumed by GET /api/v1/subscriptionPlans.
/// </summary>
public record SubscriptionPlanDashboardResource(
    string Id,
    string Name,
    string Label,
    decimal? Price,
    string Currency,
    string Interval,
    string Target,
    bool Featured,
    string ActionType,
    IEnumerable<PlanFeatureResource> Features);

/// <summary>
///     Saved payment card payload consumed by GET /api/v1/paymentMethods.
/// </summary>
public record PaymentMethodDashboardResource(
    string Id,
    string Brand,
    string Last4,
    string Holder,
    string ExpiresOn,
    bool Primary);

/// <summary>
///     Billing history row payload consumed by GET /api/v1/billingHistory.
/// </summary>
public record BillingRecordDashboardResource(
    string Id,
    string Date,
    string Description,
    decimal Amount,
    string Currency,
    string Method,
    string Status,
    string InvoiceUrl);