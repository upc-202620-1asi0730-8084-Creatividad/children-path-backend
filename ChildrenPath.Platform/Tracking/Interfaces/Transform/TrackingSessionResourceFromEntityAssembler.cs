using ChildrenPath.Platform.Tracking.Domain.Model.Aggregate;
using ChildrenPath.Platform.Tracking.Interfaces.Resources;

namespace ChildrenPath.Platform.Tracking.Interfaces.Transform;

public static class TrackingSessionResourceFromEntityAssembler
{
    public static TrackingSessionResource ToResourceFromEntity(TrackingSession entity)
    {
        var latestLocation = entity.Locations.OrderByDescending(l => l.Timestamp).FirstOrDefault();

        return new TrackingSessionResource(
            entity.Id.Value,
            entity.TripId,
            entity.VehicleId,
            entity.Status.ToString(),
            entity.StartedAt,
            latestLocation?.Latitude ?? 0,
            latestLocation?.Longitude ?? 0,
            latestLocation?.Speed ?? 0
        );
    }
}