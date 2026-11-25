namespace ChildrenPath.Platform.Assignments.Domain.Model.Queries;

/// <summary>
///     Query to retrieve all assignments associated with a specific student code.
/// </summary>
/// <param name="StudentCode">The student code to filter by (e.g., ST-001).</param>
public record GetAssignmentsByStudentCodeQuery(string StudentCode);