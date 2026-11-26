namespace ChildrenPath.Platform.Drivers.Interfaces.Resources;

public record DriverSummaryResource(
    int TotalDrivers,
    int AvailableDrivers,
    int OnRouteDrivers,
    int PendingReviews,
    int LicenseCompliance,
    int AverageSafetyScore);