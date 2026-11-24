using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Model.Queries;

namespace ChildrenPath.Platform.Fleet.Application.QueryServices;

public interface IVehicleQueryService
{
    Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesQuery query, CancellationToken cancellationToken = default);
    Task<Vehicle?> Handle(GetVehicleByIdQuery query, CancellationToken cancellationToken = default);
    Task<Vehicle?> Handle(GetVehicleByPlateQuery query, CancellationToken cancellationToken = default);
}
