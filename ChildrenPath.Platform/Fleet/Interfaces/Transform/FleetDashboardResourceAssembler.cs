using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Fleet.Interfaces.Rest.Transform;

public static class FleetDashboardResourceAssembler
{
    public static FleetSummaryResource ToSummaryResource(FleetSummary summary)
    {
        return new FleetSummaryResource(
            summary.TotalVehicles,
            summary.AvailableVehicles,
            summary.OnRouteVehicles,
            summary.MaintenanceVehicles,
            summary.AverageAvailability,
            summary.AverageCapacityUsage
        );
    }

    public static FleetDashboardResource ToDashboardResource(FleetDashboardData data)
    {
        var summaryResource = ToSummaryResource(data.Summary);
        var vehicleResources = data.Vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity);
        var alertResources = data.MaintenanceAlerts.Select(MaintenanceAlertResourceFromEntityAssembler.ToResourceFromEntity);

        return new FleetDashboardResource(
            summaryResource,
            vehicleResources,
            alertResources
        );
    }
}
