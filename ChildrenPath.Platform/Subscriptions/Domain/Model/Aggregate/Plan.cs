using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;

/// <summary>
///     Aggregate root representing a commercial subscription plan offered by Children Path.
/// </summary>
public partial class Plan
{
    public int Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public decimal? Price { get; private set; }
    public string Currency { get; private set; } = "USD";
    public string Interval { get; private set; } = "month";
    public PlanType Type { get; private set; } = PlanType.Basic;
    public int? VehicleLimit { get; private set; }
    public int? StudentLimit { get; private set; }
    public int? UserLimit { get; private set; }
    public bool IsFeatured { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string FeaturesJson { get; private set; } = "[]";

    /// <summary>
    ///     Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Plan()
    {
    }

    public Plan(
        string code,
        string name,
        decimal? price,
        PlanType type,
        int? vehicleLimit,
        int? studentLimit,
        int? userLimit = null,
        string currency = "USD",
        string interval = "month",
        bool isFeatured = false,
        string featuresJson = "[]")
    {
        Code = code.Trim().ToLowerInvariant();
        Name = name.Trim();
        Price = price is >= 0m ? price : null;
        Type = type;
        VehicleLimit = vehicleLimit;
        StudentLimit = studentLimit;
        UserLimit = userLimit;
        Currency = string.IsNullOrWhiteSpace(currency) ? "USD" : currency.Trim().ToUpperInvariant();
        Interval = string.IsNullOrWhiteSpace(interval) ? "month" : interval.Trim().ToLowerInvariant();
        IsFeatured = isFeatured;
        IsActive = true;
        FeaturesJson = string.IsNullOrWhiteSpace(featuresJson) ? "[]" : featuresJson.Trim();
    }
}