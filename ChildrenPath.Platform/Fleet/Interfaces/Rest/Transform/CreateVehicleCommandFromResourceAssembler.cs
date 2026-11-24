using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Fleet.Interfaces.Rest.Transform;

public static class CreateVehicleCommandFromResourceAssembler
{
    public static CreateVehicleCommand ToCommandFromResource(CreateVehicleResource resource)
    {
        return new CreateVehicleCommand(
            resource.Plate,
            resource.Code,
            resource.Brand,
            resource.Model,
            resource.Year,
            resource.Capacity,
            resource.AssignedStudents,
            resource.DriverName,
            resource.RouteName,
            resource.Status ?? "available",
            resource.EnergyType ?? "gasoline",
            resource.OwnershipType ?? "company",
            resource.NextMaintenanceDate,
            resource.LastInspectionDate,
            resource.MileageKm,
            resource.AvailabilityScore,
            resource.DocumentsValid,
            resource.FleetId
        );
    }
}
