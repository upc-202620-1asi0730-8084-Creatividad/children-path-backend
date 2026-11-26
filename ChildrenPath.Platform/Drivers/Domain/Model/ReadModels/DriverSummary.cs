namespace ChildrenPath.Platform.Drivers.Domain.Model.ReadModels;

public record DriverSummary(
    int TotalDrivers,
    int AvailableDrivers,
    int OnRouteDrivers,
    int PendingReviews,
    int LicenseCompliance,
    int AverageSafetyScore);