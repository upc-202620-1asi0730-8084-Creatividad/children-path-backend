namespace ChildrenPath.Platform.Assignments.Domain.Model.Commands;

/// <summary>
///     Command to mark an existing assignment as validated and ready.
/// </summary>
/// <param name="AssignmentId">The unique identifier of the assignment to validate.</param>
public record ValidateAssignmentCommand(int AssignmentId);