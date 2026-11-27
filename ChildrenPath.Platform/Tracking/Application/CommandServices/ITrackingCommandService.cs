using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Tracking.Domain.Model.Aggregate;
using ChildrenPath.Platform.Tracking.Domain.Model.Commands;

namespace ChildrenPath.Platform.Tracking.Application.CommandServices;

public interface ITrackingCommandService
{
    Task<Result<TrackingSession>> Handle(CreateTrackingSessionCommand command, CancellationToken cancellationToken);
}