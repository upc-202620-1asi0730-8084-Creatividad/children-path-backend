using ChildrenPath.Platform.Drivers.Domain.Model.Commands;
using ChildrenPath.Platform.Drivers.Interfaces.Resources;

namespace ChildrenPath.Platform.Drivers.Interfaces.Transform;

public static class UpdateDriverCommandFromResourceAssembler
{
    public static UpdateDriverCommand ToCommandFromResource(int id, UpdateDriverResource resource)
    {
        return new UpdateDriverCommand(
            id,
            resource.FullName,
            resource.Phone,
            resource.Email,
            resource.LicenseNumber,
            resource.LicenseClass,
            DriverResourceDateParser.ParseOptionalDate(resource.LicenseExpiresAt),
            resource.AssignedVehicle,
            resource.AssignedRoute,
            DriverStatusParser.Parse(resource.Status ?? "available"),
            resource.SafetyScore,
            resource.PunctualityScore,
            resource.PhotoInitials,
            resource.AvailabilityLabel,
            resource.TripsToday,
            resource.StudentsAssigned,
            resource.LastCheckIn,
            resource.DocumentsValid,
            resource.YearsOfExperience);
    }
}