using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Commands;

namespace ChildrenPath.Platform.Fleet.Application.CommandServices;

public interface IVehicleCommandService
{
    Task<Vehicle?> Handle(CreateVehicleCommand command, CancellationToken cancellationToken = default);
    Task<Vehicle?> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken = default);
    Task<bool> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken = default);
    Task SeedInitialDataAsync(CancellationToken cancellationToken = default);
}
