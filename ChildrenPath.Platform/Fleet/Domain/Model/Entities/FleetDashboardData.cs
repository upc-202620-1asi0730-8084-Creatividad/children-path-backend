using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;

namespace ChildrenPath.Platform.Fleet.Domain.Model.Entities;

public record FleetDashboardData(
    FleetSummary Summary,
    IEnumerable<Vehicle> Vehicles,
    IEnumerable<MaintenanceAlert> MaintenanceAlerts
);
