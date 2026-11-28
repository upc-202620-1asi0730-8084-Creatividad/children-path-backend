using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Fleet.Application.CommandServices;

public interface IFleetCommandService
{
    Task<Result<MaintenanceAlert>> Handle(CreateMaintenanceAlertCommand command, CancellationToken cancellationToken);
    Task<Result> Handle(ResolveMaintenanceAlertCommand command, CancellationToken cancellationToken);
    Task<Result<TransportFleet?>> Handle(CreateFleetCommand command, CancellationToken cancellationToken = default);
}
