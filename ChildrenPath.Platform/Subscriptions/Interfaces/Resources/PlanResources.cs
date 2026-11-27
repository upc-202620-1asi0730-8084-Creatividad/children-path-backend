namespace ChildrenPath.Platform.Subscriptions.Interfaces.Resources;

/// <summary>
///     Represents an individual feature line item displayed on a subscription plan card.
/// </summary>
public record PlanFeatureResource(string Text, bool Included);

/// <summary>
///     Represents a subscription plan returned by the standard REST API.
/// </summary>
public record PlanResource(
    int Id,
    string Code,
    string Name,
    decimal? Price,
    string Currency,
    string Interval,
    string Type,
    int? VehicleLimit,
    int? StudentLimit,
    int? UserLimit,
    bool IsFeatured,
    bool IsActive,
    IEnumerable<PlanFeatureResource> Features);

/// <summary>
///     Request payload for creating a new subscription plan.
/// </summary>
public record CreatePlanResource(
    string Code,
    string Name,
    decimal? Price,
    string Type,
    int? VehicleLimit,
    int? StudentLimit,
    int? UserLimit = null,
    string Currency = "USD",
    string Interval = "month",
    bool IsFeatured = false,
    IEnumerable<PlanFeatureResource>? Features = null);

/// <summary>
///     Request payload for updating an existing subscription plan.
/// </summary>
public record UpdatePlanResource(
    string Name,
    decimal? Price,
    string Type,
    int? VehicleLimit,
    int? StudentLimit,
    int? UserLimit,
    bool IsFeatured,
    IEnumerable<PlanFeatureResource>? Features = null);

/// <summary>
///     Request payload for activating or deactivating a subscription plan.
/// </summary>
public record TogglePlanStatusResource(bool IsActive);