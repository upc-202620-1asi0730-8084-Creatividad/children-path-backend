namespace ChildrenPath.Platform.Trips.Interfaces.Resources;

public record TripResource(
    int Id,
    string Code,
    string RouteName,
    string VehiclePlate,
    string DriverName,
    string School,
    string District,
    string Shift,
    string Status,
    int Capacity,
    int Students,
    string StartTime,
    string EstimatedEndTime,
    DateTime CreatedAt,
    List<RouteStopResource> Stops
);



public record RouteStopResource(
    int Id,
    string Name,
    string Address,
    int Order,
    string ScheduledTime,
    string Status
);