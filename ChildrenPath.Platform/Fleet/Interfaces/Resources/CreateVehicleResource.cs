namespace ChildrenPath.Platform.Fleet.Interfaces.Resources;

public record CreateVehicleResource(
    string PlateNumber,
    string Brand,
    string Model,
    int Capacity,
    int? FleetId,
    string DriverName,
    string RouteName,
    int AssignedStudents
);