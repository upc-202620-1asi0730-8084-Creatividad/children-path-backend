using System.Globalization;
using ChildrenPath.Platform.Drivers.Domain.Model.Entities;
using ChildrenPath.Platform.Drivers.Domain.Model.ReadModels;
using ChildrenPath.Platform.Drivers.Interfaces.Resources;

namespace ChildrenPath.Platform.Drivers.Interfaces.Transform;

public static class DriverDashboardResourceAssembler
{
    public static DriverSummaryResource ToSummaryResource(DriverSummary summary)
    {
        return new DriverSummaryResource(
            summary.TotalDrivers,
            summary.AvailableDrivers,
            summary.OnRouteDrivers,
            summary.PendingReviews,
            summary.LicenseCompliance,
            summary.AverageSafetyScore);
    }

    public static DriverReviewResource ToReviewResource(DriverReview review)
    {
        return new DriverReviewResource(
            review.Id,
            review.DriverCode,
            review.DriverName,
            review.Title,
            review.Description,
            review.DueDate,
            review.Priority);
    }

    public static DriverShiftResource ToShiftResource(DriverActivity activity)
    {
        return new DriverShiftResource(
            activity.Id,
            activity.OccurredAt.ToString("h:mm tt", CultureInfo.InvariantCulture),
            activity.Title,
            activity.Description,
            activity.DriverName,
            activity.Status);
    }
}