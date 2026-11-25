using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Tracking.Application.CommandServices;
using ChildrenPath.Platform.Tracking.Domain.Model.Aggregate;
using ChildrenPath.Platform.Tracking.Domain.Model.Commands;
using ChildrenPath.Platform.Tracking.Domain.Model.Errors;
using ChildrenPath.Platform.Tracking.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Tracking.Application.Internal.CommandServices;

public class TrackingCommandService(ITrackingSessionRepository repository, IUnitOfWork unitOfWork) 
    : ITrackingCommandService
{
    public async Task<Result<TrackingSession>> Handle(CreateTrackingSessionCommand command, CancellationToken cancellationToken)
    {
        var session = new TrackingSession(
            command.TripId, 
            command.VehicleId, 
            command.InitialLatitude, 
            command.InitialLongitude, 
            command.Speed);

        try
        {
            await repository.AddAsync(session, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<TrackingSession>.Success(session);
        }
        catch (Exception)
        {
            return Result<TrackingSession>.Failure(TrackingErrors.SessionNotFound);
        }
    }
}