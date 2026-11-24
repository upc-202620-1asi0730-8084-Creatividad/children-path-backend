namespace ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;

public record CreateVehicleResource(
    string Plate,
    string Code,
    string Brand,
    string Model,
    int Year,
    int Capacity,
    int AssignedStudents,
    string DriverName,
    string RouteName,
    string? Status,
    string? EnergyType,
    string? OwnershipType,
    DateTime? NextMaintenanceDate,
    DateTime? LastInspectionDate,
    int MileageKm,
    int AvailabilityScore,
    bool DocumentsValid,
    int? FleetId
);
