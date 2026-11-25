namespace ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;

/// <summary>
///     Represents the operational validation outcome of an assignment.
/// </summary>
public enum AssignmentValidation
{
    Ready = 0,
    CapacityRisk = 1,
    ScheduleConflict = 2,
    MissingRoute = 3,
    Inactive = 4
}