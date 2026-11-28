using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Fleet.Interfaces.Rest.Transform;

public static class VehicleResourceFromEntityAssembler
{
    public static VehicleResource ToResourceFromEntity(Vehicle entity)
    {
        return new VehicleResource(
            entity.Id,
            entity.Plate,
            entity.Code,
            entity.Brand,
            entity.Model,
            entity.Year,
            entity.Capacity,
            entity.AssignedStudents,
            entity.DriverName,
            entity.RouteName,
            entity.Status,
            entity.EnergyType,
            entity.OwnershipType,
            entity.NextMaintenanceDate,
            entity.LastInspectionDate,
            entity.MileageKm,
            entity.AvailabilityScore,
            entity.DocumentsValid,
            entity.FleetId
        );
    }
}
