using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to switch an existing subscription to another active plan.
/// </summary>
public record ChangeSubscriptionPlanCommand(
    int SubscriptionId,
    int NewPlanId,
    int DurationInMonths,
    PaymentMethod PaymentMethod);