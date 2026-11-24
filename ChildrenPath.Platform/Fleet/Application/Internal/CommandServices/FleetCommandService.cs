using ChildrenPath.Platform.Fleet.Application.CommandServices;
using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Fleet.Application.Internal.CommandServices;

public class FleetCommandService(
    IMaintenanceAlertRepository maintenanceAlertRepository,
    IFleetRepository fleetRepository,
    IUnitOfWork unitOfWork) : IFleetCommandService
{
    public async Task<MaintenanceAlert?> Handle(CreateMaintenanceAlertCommand command, CancellationToken cancellationToken = default)
    {
        var alert = new MaintenanceAlert(command);
        await maintenanceAlertRepository.AddAsync(alert, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return alert;
    }

    public async Task<bool> Handle(ResolveMaintenanceAlertCommand command, CancellationToken cancellationToken = default)
    {
        var alert = await maintenanceAlertRepository.FindByIdAsync(command.Id, cancellationToken);
        if (alert == null) return false;

        alert.Resolve();
        maintenanceAlertRepository.Update(alert);
        await unitOfWork.CompleteAsync(cancellationToken);
        return true;
    }

    public async Task<TransportFleet?> Handle(CreateFleetCommand command, CancellationToken cancellationToken = default)
    {
        var fleet = new TransportFleet(command);
        await fleetRepository.AddAsync(fleet, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return fleet;
    }
}
