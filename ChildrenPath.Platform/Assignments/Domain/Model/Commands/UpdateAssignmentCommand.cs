using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Assignments.Domain.Model.Commands;

/// <summary>
///     Command to update an existing student transport assignment.
/// </summary>
public record UpdateAssignmentCommand(
    int Id,
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
    int ValidationScore,
    AssignmentValidation Validation,
    AssignmentStatus Status,
    string Notes);