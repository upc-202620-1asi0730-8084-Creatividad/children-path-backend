using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to update the details of an existing subscription plan.
/// </summary>
public record UpdatePlanCommand(
    int PlanId,
    string Name,
    decimal? Price,
    PlanType Type,
    int? VehicleLimit,
    int? StudentLimit,
    int? UserLimit,
    bool IsFeatured,
    string? FeaturesJson = null);