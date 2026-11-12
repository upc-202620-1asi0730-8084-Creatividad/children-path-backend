using ChildrenPath.Platform.Trips.Domain.Model.Aggregate;
using ChildrenPath.Platform.Trips.Interfaces.Resources;

namespace ChildrenPath.Platform.Trips.Interfaces.Transform;

public static class TripResourceFromEntityAssembler
{
    public static TripResource ToResourceFromEntity(Trip entity)
    {
        return new TripResource(
            entity.Id,
            entity.Code,
            entity.RouteName,
            entity.VehiclePlate,
            entity.DriverName,
            entity.School,
            entity.District,
            entity.Shift.ToString().ToLower(),
            entity.Status.ToString().ToLower(),
            entity.Capacity,
            entity.Students,
            entity.StartTime,
            entity.EstimatedEndTime,
            entity.CreatedAt,
            entity.Stops.OrderBy(s => s.Order).Select(s => new RouteStopResource(
                s.Id,
                s.Name,
                s.Address,
                s.Order,
                s.ScheduledTime,
                s.Status.ToString().ToLower()
            )).ToList()
        );
    }
}