using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Fleet.Interfaces.Rest.Transform;

public static class MaintenanceAlertResourceFromEntityAssembler
{
    public static MaintenanceAlertResource ToResourceFromEntity(MaintenanceAlert entity)
    {
        return new MaintenanceAlertResource(
            entity.Id,
            entity.VehicleCode,
            entity.Plate,
            entity.Title,
            entity.DueDate,
            entity.Priority,
            entity.Description,
            entity.VehicleId,
            entity.IsResolved
        );
    }
}
