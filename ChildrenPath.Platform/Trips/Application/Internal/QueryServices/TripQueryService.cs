using ChildrenPath.Platform.Trips.Application.QueryServices;
using ChildrenPath.Platform.Trips.Domain.Model.Aggregate;
using ChildrenPath.Platform.Trips.Domain.Model.Queries;
using ChildrenPath.Platform.Trips.Domain.Repositories;

namespace ChildrenPath.Platform.Trips.Application.Internal.QueryServices;

public class TripQueryService(ITripRepository tripRepository) : ITripQueryService
{
    public async Task<Trip?> Handle(GetTripByIdQuery query, CancellationToken cancellationToken)
    {
        return await tripRepository.FindByIdAsync(query.TripId, cancellationToken);
    }

    public async Task<IEnumerable<Trip>> Handle(GetAllTripsQuery query, CancellationToken cancellationToken)
    {
        return await tripRepository.ListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Trip>> Handle(GetTripsByStatusQuery query, CancellationToken cancellationToken)
    {
        return await tripRepository.FindByStatusAsync(query.Status, cancellationToken);
    }

    public async Task<IEnumerable<Trip>> Handle(GetTripsByShiftQuery query, CancellationToken cancellationToken)
    {
        return await tripRepository.FindByShiftAsync(query.Shift, cancellationToken);
    }
}