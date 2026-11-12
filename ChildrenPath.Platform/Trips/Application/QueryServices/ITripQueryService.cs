using ChildrenPath.Platform.Trips.Domain.Model.Aggregate;
using ChildrenPath.Platform.Trips.Domain.Model.Queries;

namespace ChildrenPath.Platform.Trips.Application.QueryServices;

/// <summary>
///     Represents the trip query service contract.
/// </summary>
///
/// 
public interface ITripQueryService
{
    Task<Trip?> Handle(GetTripByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Trip>> Handle(GetAllTripsQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Trip>> Handle(GetTripsByStatusQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Trip>> Handle(GetTripsByShiftQuery query, CancellationToken cancellationToken);
}