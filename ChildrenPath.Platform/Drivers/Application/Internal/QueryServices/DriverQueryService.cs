using ChildrenPath.Platform.Drivers.Application.QueryServices;
using ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;
using ChildrenPath.Platform.Drivers.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Drivers.Domain.Model.Entities;
using ChildrenPath.Platform.Drivers.Domain.Model.Queries;
using ChildrenPath.Platform.Drivers.Domain.Model.ReadModels;
using ChildrenPath.Platform.Drivers.Domain.Repositories;

namespace ChildrenPath.Platform.Drivers.Application.Internal.QueryServices;

public class DriverQueryService(
    IDriverRepository driverRepository,
    IDriverActivityRepository activityRepository) : IDriverQueryService
{
    public async Task<IEnumerable<Driver>> Handle(GetAllDriversQuery query, CancellationToken cancellationToken = default)
        => await driverRepository.ListWithRelationsAsync(cancellationToken);

    public async Task<Driver?> Handle(GetDriverByIdQuery query, CancellationToken cancellationToken = default)
        => await driverRepository.FindWithRelationsByIdAsync(query.Id, cancellationToken);

    public async Task<DriverSummary> Handle(GetDriverSummaryQuery query, CancellationToken cancellationToken = default)
    {
        var drivers = (await driverRepository.ListWithRelationsAsync(cancellationToken)).ToList();
        var totalDrivers = drivers.Count;
        var reviews = BuildReviews(drivers);
        var licenseCompliant = drivers.Count(IsLicenseCompliant);

        return new DriverSummary(
            TotalDrivers: totalDrivers,
            AvailableDrivers: drivers.Count(driver => driver.Status == DriverStatus.Available),
            OnRouteDrivers: drivers.Count(driver => driver.Status == DriverStatus.OnRoute),
            PendingReviews: reviews.Count,
            LicenseCompliance: totalDrivers == 0
                ? 0
                : (int)Math.Round((double)licenseCompliant * 100 / totalDrivers, MidpointRounding.AwayFromZero),
            AverageSafetyScore: totalDrivers == 0
                ? 0
                : (int)Math.Round(drivers.Average(driver => driver.SafetyScore), MidpointRounding.AwayFromZero));
    }

    public async Task<IEnumerable<DriverReview>> Handle(
        GetDriverReviewsQuery query,
        CancellationToken cancellationToken = default)
    {
        var drivers = await driverRepository.ListWithRelationsAsync(cancellationToken);
        return BuildReviews(drivers);
    }

    public async Task<IEnumerable<DriverActivity>> Handle(
        GetDriverShiftsQuery query,
        CancellationToken cancellationToken = default)
    {
        var limit = Math.Clamp(query.Limit, 0, 100);
        return await activityRepository.ListRecentAsync(limit, cancellationToken);
    }

    private static List<DriverReview> BuildReviews(IEnumerable<Driver> drivers)
    {
        var today = DateTime.UtcNow.Date;
        var reviews = new List<DriverReview>();

        foreach (var driver in drivers)
        {
            var licenseExpired = driver.LicenseExpiry.HasValue && driver.LicenseExpiry.Value.Date < today;
            if (driver.DocumentsValid && !licenseExpired &&
                driver.Status is not (DriverStatus.Review or DriverStatus.Suspended))
                continue;

            var title = licenseExpired
                ? "Driver license has expired"
                : !driver.DocumentsValid
                    ? "Driver document review required"
                    : "Driver profile requires review";
            var description = licenseExpired
                ? "Renew the driver's license before assigning new routes."
                : !driver.DocumentsValid
                    ? "Validate the driver's documents before assigning new routes."
                    : "Review the driver's profile and confirm the requirements for route assignment.";

            reviews.Add(new DriverReview(
                Id: $"driver-review-{driver.Id}",
                DriverCode: driver.Code,
                DriverName: driver.FullName,
                Title: title,
                Description: description,
                DueDate: driver.LicenseExpiry,
                Priority: licenseExpired || !driver.DocumentsValid ? "high" : "medium"));
        }

        return reviews;
    }

    private static bool IsLicenseCompliant(Driver driver)
    {
        return driver.DocumentsValid &&
               (!driver.LicenseExpiry.HasValue || driver.LicenseExpiry.Value.Date >= DateTime.UtcNow.Date);
    }
}