namespace ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;

public record MaintenanceAlertResource(
    int Id,
    string VehicleCode,
    string Plate,
    string Title,
    string DueDate,
    string Priority,
    string Description,
    int? VehicleId,
    bool IsResolved
);
