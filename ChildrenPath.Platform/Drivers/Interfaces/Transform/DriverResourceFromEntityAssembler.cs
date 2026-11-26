using ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;
using ChildrenPath.Platform.Drivers.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Drivers.Interfaces.Resources;

namespace ChildrenPath.Platform.Drivers.Interfaces.Transform;

public static class DriverResourceFromEntityAssembler
{
    public static DriverResource ToResourceFromEntity(Driver driver)
    {
        return new DriverResource(
            driver.Id,
            driver.Code,
            driver.FullName,
            driver.PhotoInitials,
            driver.LicenseNumber,
            driver.LicenseClass,
            driver.LicenseExpiry,
            driver.Phone,
            driver.Email,
            driver.AssignedVehicle,
            driver.AssignedRoute,
            ToClientStatus(driver.Status),
            driver.AvailabilityLabel,
            driver.TripsToday,
            driver.StudentsAssigned,
            driver.SafetyScore,
            driver.PunctualityScore,
            driver.LastCheckIn,
            driver.DocumentsValid,
            driver.YearsOfExperience);
    }

    private static string ToClientStatus(DriverStatus status)
    {
        return status switch
        {
            DriverStatus.OnRoute => "onRoute",
            DriverStatus.OffDuty => "offDuty",
            DriverStatus.Review or DriverStatus.Suspended => "review",
            _ => "available"
        };
    }
}