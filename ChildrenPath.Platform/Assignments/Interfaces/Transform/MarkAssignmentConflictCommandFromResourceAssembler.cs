using ChildrenPath.Platform.Assignments.Domain.Model.Commands;
using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Assignments.Interfaces.Resources;

namespace ChildrenPath.Platform.Assignments.Interfaces.Transform;

/// <summary>
///     Assembles <see cref="MarkAssignmentConflictCommand" /> instances from <see cref="MarkAssignmentConflictResource" /> payloads.
/// </summary>
public static class MarkAssignmentConflictCommandFromResourceAssembler
{
    public static MarkAssignmentConflictCommand ToCommandFromResource(
        int assignmentId,
        MarkAssignmentConflictResource resource)
    {
        var validation = string.IsNullOrWhiteSpace(resource.Validation)
            ? AssignmentValidation.ScheduleConflict
            : AssignmentEnumMapper.ParseValidation(resource.Validation);

        return new MarkAssignmentConflictCommand(
            assignmentId,
            resource.Reason,
            validation);
    }
}