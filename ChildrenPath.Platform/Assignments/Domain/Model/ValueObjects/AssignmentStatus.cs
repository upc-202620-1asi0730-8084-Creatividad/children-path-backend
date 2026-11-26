namespace ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;

/// <summary>
///     Represents the lifecycle status of a student transport assignment.
/// </summary>
public enum AssignmentStatus
{
    Validated = 0,
    Pending = 1,
    Conflict = 2,
    Inactive = 3
}