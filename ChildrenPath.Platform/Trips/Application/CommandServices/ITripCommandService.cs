using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Trips.Domain.Model.Aggregate;
using ChildrenPath.Platform.Trips.Domain.Model.Commands;

namespace ChildrenPath.Platform.Trips.Application.CommandServices;

/// <summary>
///     Represents the trip command service contract.
/// </summary>
public interface ITripCommandService
{
    Task<Result<Trip>> Handle(CreateTripCommand command, CancellationToken cancellationToken);
    Task<Result<Trip>> Handle(StartTripCommand command, CancellationToken cancellationToken);
    Task<Result<Trip>> Handle(CompleteTripCommand command, CancellationToken cancellationToken);
    Task<Result<Trip>> Handle(CancelTripCommand command, CancellationToken cancellationToken);
    Task<Result<Trip>> Handle(MarkTripAsDelayedCommand command, CancellationToken cancellationToken);
    Task<Result<Trip>> Handle(AddRouteStopCommand command, CancellationToken cancellationToken);
}