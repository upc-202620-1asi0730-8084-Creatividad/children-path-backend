namespace ChildrenPath.Platform.Fleet.Domain.Model.Entities;

public record FleetSummary(
    int TotalVehicles,
    int AvailableVehicles,
    int OnRouteVehicles,
    int MaintenanceVehicles,
    int AverageAvailability,
    int AverageCapacityUsage
);
