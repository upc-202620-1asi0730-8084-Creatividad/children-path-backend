namespace ChildrenPath.Platform.Fleet.Interfaces.Resources;

public record CreateVehicleResource(
    string Plate,
    string Code,
    string Brand,
    string Model,
    int Year,
    int Capacity,
    string DriverName,
    string RouteName,
    int AssignedStudents,
    string Status
);