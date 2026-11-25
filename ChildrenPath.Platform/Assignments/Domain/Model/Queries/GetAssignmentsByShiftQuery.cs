using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Assignments.Domain.Model.Queries;

/// <summary>
///     Query to retrieve all assignments scheduled for a specific shift.
/// </summary>
/// <param name="Shift">The target <see cref="AssignmentShift" />.</param>
public record GetAssignmentsByShiftQuery(AssignmentShift Shift);