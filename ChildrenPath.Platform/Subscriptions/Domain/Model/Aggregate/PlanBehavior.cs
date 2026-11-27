using ChildrenPath.Platform.Subscriptions.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Subscriptions.Domain.Model.Aggregate;

public partial class Plan
{
    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public void UpdateDetails(
        string name,
        decimal? price,
        PlanType type,
        int? vehicleLimit,
        int? studentLimit,
        int? userLimit,
        bool isFeatured,
        string? featuresJson = null)
    {
        Name = name.Trim();
        Price = price is >= 0m ? price : null;
        Type = type;
        VehicleLimit = vehicleLimit;
        StudentLimit = studentLimit;
        UserLimit = userLimit;
        IsFeatured = isFeatured;

        if (!string.IsNullOrWhiteSpace(featuresJson))
            FeaturesJson = featuresJson.Trim();
    }
}