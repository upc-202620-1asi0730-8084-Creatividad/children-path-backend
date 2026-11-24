namespace ChildrenPath.Platform.Fleet.Domain.Model.Commands;

public record CreateMaintenanceAlertCommand(
    string VehicleCode,
    string Plate,
    string Title,
    string DueDate,
    string Priority,
    string Description,
    int? VehicleId
);
