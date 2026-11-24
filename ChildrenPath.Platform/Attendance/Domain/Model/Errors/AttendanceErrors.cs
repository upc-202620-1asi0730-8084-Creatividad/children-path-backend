using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Attendance.Domain.Model.Errors;

/// <summary>
///     Contains domain and application error definitions for the Attendance Bounded Context.
/// </summary>
public static class AttendanceErrors
{
    /// <summary>
    ///     Error returned when a requested attendance record cannot be found.
    /// </summary>
    public static readonly Error AttendanceNotFound =
        new("Attendance.NotFound", "The specified attendance record was not found.");

    /// <summary>
    ///     Error returned when an attendance record already exists for the same student and trip.
    /// </summary>
    public static readonly Error AttendanceAlreadyExists =
        new("Attendance.AlreadyExists", "An attendance record already exists for this student on this trip.");

    /// <summary>
    ///     Error returned when an unsupported or invalid attendance status is provided.
    /// </summary>
    public static readonly Error InvalidStatus =
        new("Attendance.InvalidStatus", "The provided attendance status is invalid.");

    /// <summary>
    ///     Error returned when the requested operation is cancelled before completion.
    /// </summary>
    public static readonly Error OperationCancelled =
        new("Attendance.OperationCancelled", "The operation was cancelled.");

    /// <summary>
    ///     Error returned when a database persistence failure occurs.
    /// </summary>
    public static readonly Error DatabaseError =
        new("Attendance.DatabaseError", "A database error occurred while processing the request.");

    /// <summary>
    ///     Error returned when an unexpected server error occurs.
    /// </summary>
    public static readonly Error InternalServerError =
        new("Attendance.InternalServerError", "An internal server error occurred.");
}