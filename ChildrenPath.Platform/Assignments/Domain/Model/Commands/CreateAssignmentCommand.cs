using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Assignments.Domain.Model.Commands;

/// <summary>
///     Command to create a new student transport assignment.
/// </summary>
public record CreateAssignmentCommand(
    string StudentCode,
    string StudentName,
    string Grade,
    string GuardianName,
    string RouteCode,
    string RouteName,
    string VehiclePlate,
    string DriverName,
    AssignmentShift Shift,
    string PickupPoint,
    string PickupWindow,
    int CapacityUsage,
    string Notes,
    int? ValidationScore = null,
    AssignmentValidation? Validation = null,
    AssignmentStatus? Status = null);