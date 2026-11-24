using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Fleet.Domain.Model.Entities;

namespace ChildrenPath.Platform.Fleet.Application.CommandServices;

public interface IFleetCommandService
{
    Task<MaintenanceAlert?> Handle(CreateMaintenanceAlertCommand command, CancellationToken cancellationToken = default);
    Task<bool> Handle(ResolveMaintenanceAlertCommand command, CancellationToken cancellationToken = default);
    Task<TransportFleet?> Handle(CreateFleetCommand command, CancellationToken cancellationToken = default);
}
