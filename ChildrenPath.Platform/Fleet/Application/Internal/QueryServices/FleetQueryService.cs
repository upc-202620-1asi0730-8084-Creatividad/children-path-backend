using ChildrenPath.Platform.Fleet.Application.CommandServices;
using ChildrenPath.Platform.Fleet.Application.QueryServices;
using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Domain.Model.Queries;
using ChildrenPath.Platform.Fleet.Domain.Repositories;

namespace ChildrenPath.Platform.Fleet.Application.Internal.QueryServices;

public class FleetQueryService(
    IVehicleRepository vehicleRepository,
    IMaintenanceAlertRepository maintenanceAlertRepository,
    IVehicleCommandService vehicleCommandService) : IFleetQueryService
{
    public async Task<FleetSummary> Handle(GetFleetSummaryQuery query, CancellationToken cancellationToken = default)
    {
        var vehicles = (await vehicleRepository.ListAsync(cancellationToken)).ToList();
        if (vehicles.Count == 0)
        {
            await vehicleCommandService.SeedInitialDataAsync(cancellationToken);
            vehicles = (await vehicleRepository.ListAsync(cancellationToken)).ToList();
        }

        var totalVehicles = vehicles.Count;
        var availableVehicles = vehicles.Count(v => v.Status.Equals("available", StringComparison.OrdinalIgnoreCase));
        var onRouteVehicles = vehicles.Count(v => v.Status.Equals("on_route", StringComparison.OrdinalIgnoreCase));
        var maintenanceVehicles = vehicles.Count(v => v.Status.Equals("maintenance", StringComparison.OrdinalIgnoreCase));
        var averageAvailability = totalVehicles > 0 ? (int)Math.Round(vehicles.Average(v => v.AvailabilityScore)) : 95;
        
        var totalCapacity = vehicles.Sum(v => v.Capacity);
        var totalAssignedStudents = vehicles.Sum(v => v.AssignedStudents);
        var averageCapacityUsage = totalCapacity > 0 ? (int)Math.Round((double)totalAssignedStudents * 100 / totalCapacity) : 85;

        return new FleetSummary(
            TotalVehicles: totalVehicles,
            AvailableVehicles: availableVehicles,
            OnRouteVehicles: onRouteVehicles,
            MaintenanceVehicles: maintenanceVehicles,
            AverageAvailability: averageAvailability,
            AverageCapacityUsage: averageCapacityUsage
        );
    }

    public async Task<IEnumerable<MaintenanceAlert>> Handle(GetAllMaintenanceAlertsQuery query, CancellationToken cancellationToken = default)
    {
        var alerts = (await maintenanceAlertRepository.ListAsync(cancellationToken)).ToList();
        if (alerts.Count == 0)
        {
            await vehicleCommandService.SeedInitialDataAsync(cancellationToken);
            alerts = (await maintenanceAlertRepository.ListAsync(cancellationToken)).ToList();
        }
        return alerts;
    }

    public async Task<FleetDashboardData> Handle(GetFleetDashboardQuery query, CancellationToken cancellationToken = default)
    {
        var summary = await Handle(new GetFleetSummaryQuery(), cancellationToken);
        var vehicles = (await vehicleRepository.ListAsync(cancellationToken)).ToList();
        var alerts = (await maintenanceAlertRepository.ListAsync(cancellationToken)).ToList();

        return new FleetDashboardData(summary, vehicles, alerts);
    }
    
    public async Task<MaintenanceAlert?> Handle(GetMaintenanceAlertByIdQuery query, CancellationToken cancellationToken)
    {
        return await maintenanceAlertRepository.FindByIdAsync(query.Id, cancellationToken);
    }
    
}
