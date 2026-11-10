namespace ChildrenPath.Platform.Assignments.Interfaces.Resources;

public record CreateAssignmentResource(
    string StudentCode,
    string StudentName,
    string Grade,
    string GuardianName,
    string RouteCode,
    string RouteName,
    string VehiclePlate,
    string DriverName,
    string Shift,
    string PickupPoint,
    string PickupWindow,
    int CapacityUsage,
    string Notes
);