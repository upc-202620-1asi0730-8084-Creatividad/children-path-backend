using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Fleet.Interfaces.Rest.Transform;

public static class UpdateVehicleCommandFromResourceAssembler
{
    public static UpdateVehicleCommand ToCommandFromResource(int id, UpdateVehicleResource resource)
    {
        return new UpdateVehicleCommand(
            id,
            resource.Plate,
            resource.Code,
            resource.Brand,
            resource.Model,
            resource.Year,
            resource.Capacity,
            resource.AssignedStudents,
            resource.DriverName,
            resource.RouteName,
            resource.Status,
            resource.EnergyType,
            resource.OwnershipType,
            resource.NextMaintenanceDate,
            resource.LastInspectionDate,
            resource.MileageKm,
            resource.AvailabilityScore,
            resource.DocumentsValid,
            resource.FleetId
        );
    }
}
