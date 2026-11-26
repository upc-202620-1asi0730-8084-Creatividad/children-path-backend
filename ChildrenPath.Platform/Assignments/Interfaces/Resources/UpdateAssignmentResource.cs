namespace ChildrenPath.Platform.Assignments.Interfaces.Resources;

/// <summary>
///     Request payload for updating an existing student transport assignment.
/// </summary>
public record UpdateAssignmentResource(
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
    string Notes = "");