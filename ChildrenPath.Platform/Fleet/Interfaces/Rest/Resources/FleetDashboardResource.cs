namespace ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;

public record FleetDashboardResource(
    FleetSummaryResource Summary,
    IEnumerable<VehicleResource> Vehicles,
    IEnumerable<MaintenanceAlertResource> MaintenanceAlerts
);
