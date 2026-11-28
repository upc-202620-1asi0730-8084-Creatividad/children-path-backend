using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Commands;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Fleet.Application.CommandServices;

public interface IVehicleCommandService
{
    Task SeedInitialDataAsync(CancellationToken cancellationToken = default);
    Task<Result<Vehicle>> Handle(CreateVehicleCommand command, CancellationToken cancellationToken = default);
    Task<Result<Vehicle>> Handle(UpdateVehicleCommand command, CancellationToken cancellationToken = default);
    Task<Result> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken = default);
}
