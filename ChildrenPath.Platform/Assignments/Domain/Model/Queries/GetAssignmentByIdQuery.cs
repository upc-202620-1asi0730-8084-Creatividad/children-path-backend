namespace ChildrenPath.Platform.Assignments.Domain.Model.Queries;

/// <summary>
///     Query to retrieve a single assignment record by its identifier.
/// </summary>
/// <param name="Id">The unique identifier of the assignment.</param>
public record GetAssignmentByIdQuery(int Id);