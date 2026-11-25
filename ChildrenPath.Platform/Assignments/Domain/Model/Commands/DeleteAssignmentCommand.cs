namespace ChildrenPath.Platform.Assignments.Domain.Model.Commands;

/// <summary>
///     Command to delete an existing assignment record.
/// </summary>
/// <param name="AssignmentId">The unique identifier of the assignment to remove.</param>
public record DeleteAssignmentCommand(int AssignmentId);