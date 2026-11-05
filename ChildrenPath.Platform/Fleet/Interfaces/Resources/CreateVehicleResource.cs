namespace ChildrenPath.Platform.Fleet.Interfaces.Resources;

public record CreateVehicleResource(
    string PlateNumber,
    string Brand,
    string Model,
    int Capacity,
    string DriverName,
    string RouteName,
    int AssignedStudents
);