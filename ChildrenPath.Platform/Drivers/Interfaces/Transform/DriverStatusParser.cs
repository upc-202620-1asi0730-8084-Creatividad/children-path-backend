using ChildrenPath.Platform.Drivers.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Drivers.Interfaces.Transform;

internal static class DriverStatusParser
{
    public static DriverStatus Parse(string? status)
    {
        var normalized = status?.Trim().Replace("-", string.Empty).Replace("_", string.Empty).ToLowerInvariant();
        return normalized switch
        {
            "available" => DriverStatus.Available,
            "onroute" => DriverStatus.OnRoute,
            "offduty" => DriverStatus.OffDuty,
            "review" => DriverStatus.Review,
            _ => throw new InvalidOperationException("Status must be available, onRoute, offDuty, or review.")
        };
    }
}