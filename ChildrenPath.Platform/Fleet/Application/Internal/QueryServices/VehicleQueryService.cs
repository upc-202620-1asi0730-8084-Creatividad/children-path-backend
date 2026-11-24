using ChildrenPath.Platform.Fleet.Application.CommandServices;
using ChildrenPath.Platform.Fleet.Application.QueryServices;
using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Queries;
using ChildrenPath.Platform.Fleet.Domain.Repositories;

namespace ChildrenPath.Platform.Fleet.Application.Internal.QueryServices;

public class VehicleQueryService(
    IVehicleRepository vehicleRepository,
    IVehicleCommandService vehicleCommandService) : IVehicleQueryService
{
    public async Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesQuery query, CancellationToken cancellationToken = default)
    {
        var vehicles = (await vehicleRepository.ListAsync(cancellationToken)).ToList();
        if (vehicles.Count == 0)
        {
            await vehicleCommandService.SeedInitialDataAsync(cancellationToken);
            vehicles = (await vehicleRepository.ListAsync(cancellationToken)).ToList();
        }
        return vehicles;
    }

    public async Task<Vehicle?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await vehicleRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    public async Task<Vehicle?> Handle(GetVehicleByPlateQuery query, CancellationToken cancellationToken = default)
    {
        return await vehicleRepository.FindByPlateAsync(query.Plate, cancellationToken);
    }
}
