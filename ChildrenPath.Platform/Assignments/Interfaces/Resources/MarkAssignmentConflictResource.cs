namespace ChildrenPath.Platform.Assignments.Interfaces.Resources;

/// <summary>
///     Request payload for flagging an assignment with an operational conflict.
/// </summary>
/// <param name="Reason">The explanation describing the conflict.</param>
/// <param name="Validation">The validation state classification (e.g., schedule-conflict, capacity-risk).</param>
public record MarkAssignmentConflictResource(
    string Reason,
    string? Validation = "schedule-conflict");