namespace ChildrenPath.Platform.Assignments.Interfaces.Resources;

/// <summary>
///     Represents an assignment record returned by the standard REST API.
/// </summary>
public record AssignmentResource(
    int Id,
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
    int ValidationScore,
    string Validation,
    string Status,
    DateTime LastUpdated,
    string Notes);