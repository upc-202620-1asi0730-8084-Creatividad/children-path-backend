namespace ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

/// <summary>
///     Request payload for creating a new subscription along with its initial invoice and payment.
/// </summary>
public record CreateSubscriptionResource(
    int UserId,
    int PlanId,
    int DurationInMonths,
    string PaymentMethod,
    string? ContractOwner = null,
    string? BillingEmail = null,
    string? RenewalMode = "assisted",
    string? SupportContact = null,
    string? PlanScope = null,
    int VehiclesUsed = 0,
    int StudentsUsed = 0,
    int UsersUsed = 1);