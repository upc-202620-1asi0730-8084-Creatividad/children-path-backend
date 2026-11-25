using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Assignments.Domain.Model.Errors;

/// <summary>
///     Centralized domain error definitions for the Assignments bounded context.
/// </summary>
public static class AssignmentErrors
{
    public static readonly Error AssignmentNotFound = Error.NotFound(
        "Assignments.NotFound",
        "The specified assignment record was not found.");

    public static readonly Error AssignmentAlreadyExists = Error.Conflict(
        "Assignments.AlreadyExists",
        "An assignment for this student in the selected shift already exists.");

    public static readonly Error InvalidShift = Error.Validation(
        "Assignments.InvalidShift",
        "The provided assignment shift is not valid.");

    public static readonly Error InvalidStatus = Error.Validation(
        "Assignments.InvalidStatus",
        "The provided assignment status is not valid.");

    public static readonly Error InvalidValidationState = Error.Validation(
        "Assignments.InvalidValidationState",
        "The provided assignment validation state is not valid.");

    public static readonly Error InvalidAssignmentData = Error.Validation(
        "Assignments.InvalidData",
        "Student code, student name, route code, and route name are required.");

    public static readonly Error OperationCancelled = Error.Conflict(
        "Assignments.OperationCancelled",
        "The operation was cancelled.");

    public static readonly Error DatabaseError = Error.Unexpected(
        "Assignments.DatabaseError",
        "A database error occurred while processing the assignment.");

    public static readonly Error InternalServerError = Error.Unexpected(
        "Assignments.InternalServerError",
        "An unexpected error occurred while processing the assignment.");
}