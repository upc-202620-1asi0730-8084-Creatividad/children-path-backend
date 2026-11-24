namespace ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;

public record FleetSummaryResource(
    int TotalVehicles,
    int AvailableVehicles,
    int OnRouteVehicles,
    int MaintenanceVehicles,
    int AverageAvailability,
    int AverageCapacityUsage
);
