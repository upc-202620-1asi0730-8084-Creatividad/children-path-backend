using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Attendance.Domain.Model.Errors;

/// <summary>
///     Contains domain and application error definitions for the Attendance Bounded Context.
/// </summary>
public static class AttendanceErrors
{
    public static readonly Error AttendanceNotFound      = Error.NotFound("Attendance.NotFound", "Attendance record not found.");
    public static readonly Error AttendanceAlreadyExists = Error.Conflict("Attendance.AlreadyExists", "Attendance record already exists.");
    public static readonly Error InvalidStatus           = Error.Validation("Attendance.InvalidStatus", "Attendance status is invalid.");
    public static readonly Error OperationCancelled      = Error.Conflict("Attendance.OperationCancelled", "Operation was cancelled.");
    public static readonly Error DatabaseError           = Error.Unexpected("Attendance.DatabaseError", "A database error occurred.");
    public static readonly Error InternalServerError     = Error.Unexpected("Attendance.InternalServerError", "An internal error occurred.");
}