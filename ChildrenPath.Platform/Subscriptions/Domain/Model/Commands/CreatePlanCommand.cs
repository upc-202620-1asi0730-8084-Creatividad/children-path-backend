using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Commands;

/// <summary>
///     Command to create a new commercial subscription plan.
/// </summary>
public record CreatePlanCommand(
    string Code,
    string Name,
    decimal? Price,
    PlanType Type,
    int? VehicleLimit,
    int? StudentLimit,
    int? UserLimit = null,
    string Currency = "USD",
    string Interval = "month",
    bool IsFeatured = false,
    string FeaturesJson = "[]");