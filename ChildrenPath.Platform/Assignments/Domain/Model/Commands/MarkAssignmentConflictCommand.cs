using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Assignments.Domain.Model.Commands;

/// <summary>
///     Command to flag an existing assignment as having an operational conflict.
/// </summary>
/// <param name="AssignmentId">The unique identifier of the assignment.</param>
/// <param name="Reason">The explanation for the detected conflict.</param>
/// <param name="Validation">The specific validation issue classification.</param>
public record MarkAssignmentConflictCommand(
    int AssignmentId,
    string Reason,
    AssignmentValidation Validation = AssignmentValidation.ScheduleConflict);