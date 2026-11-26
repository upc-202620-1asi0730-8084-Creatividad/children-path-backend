namespace ChildrenPath.Platform.Assignments.Interfaces.Resources;

/// <summary>
///     Request payload for creating a new student transport assignment.
/// </summary>
public record CreateAssignmentResource(
    string StudentCode,
    string StudentName,
    string Grade,
    string GuardianName,
    string RouteCode,
    string RouteName,
    string VehiclePlate,
    string DriverName,
    string Shift = "morning",
    string PickupPoint = "",
    string PickupWindow = "",
    int CapacityUsage = 0,
    string Notes = "",
    int? ValidationScore = null,
    string? Validation = null,
    string? Status = null);