namespace ChildrenPath.Platform.Trips.Interfaces.Resources;

public record CreateTripResource(
    string Code,
    string RouteName,
    string VehiclePlate,
    string DriverName,
    string School,
    string District,
    string Shift,
    string StartTime,
    string EstimatedEndTime,
    int Capacity
);



public record AddRouteStopResource(
    string Name,
    string Address,
    int Order,
    string ScheduledTime
);