using ChildrenPath.Platform.Trips.Domain.Model.Commands;
using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Trips.Interfaces.Resources;

namespace ChildrenPath.Platform.Trips.Interfaces.Transform;

public static class CreateTripCommandFromResourceAssembler
{
    public static CreateTripCommand ToCommandFromResource(CreateTripResource resource)
    {
        if (!Enum.TryParse<TripShift>(resource.Shift, true, out var shift))
            shift = TripShift.Morning;

        return new CreateTripCommand(
            resource.Code,
            resource.RouteName,
            resource.VehiclePlate,
            resource.DriverName,
            resource.School,
            resource.District,
            shift,
            resource.StartTime,
            resource.EstimatedEndTime,
            resource.Capacity
        );
    }
}