using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to create a new subscription and generate its initial invoice and payment.
/// </summary>
public record CreateSubscriptionCommand(
    int UserId,
    int PlanId,
    int DurationInMonths,
    PaymentMethod PaymentMethod,
    string? ContractOwner = null,
    string? BillingEmail = null,
    RenewalMode RenewalMode = RenewalMode.Assisted,
    string? SupportContact = null,
    string? PlanScope = null,
    int VehiclesUsed = 0,
    int StudentsUsed = 0,
    int UsersUsed = 1);