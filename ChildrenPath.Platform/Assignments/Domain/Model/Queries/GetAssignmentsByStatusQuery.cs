using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Assignments.Domain.Model.Queries;

/// <summary>
///     Query to retrieve all assignments matching a specific lifecycle status.
/// </summary>
/// <param name="Status">The target <see cref="AssignmentStatus" />.</param>
public record GetAssignmentsByStatusQuery(AssignmentStatus Status);