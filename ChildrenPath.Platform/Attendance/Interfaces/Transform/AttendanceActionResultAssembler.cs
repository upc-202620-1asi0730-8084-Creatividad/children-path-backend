using ChildrenPath.Platform.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Attendance.Interfaces.Transform;

/// <summary>
///     Translates <see cref="Result{T}" /> outcomes from Attendance services into HTTP <see cref="IActionResult" /> responses.
/// </summary>
public static class AttendanceActionResultAssembler
{
    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess)
            return successAction(result.Value!);

        var code = result.Error.Code;

        var statusCode = code switch
        {
            "Attendance.NotFound"           => StatusCodes.Status404NotFound,
            "Attendance.AlreadyExists"      => StatusCodes.Status409Conflict,
            "Attendance.InvalidStatus"      => StatusCodes.Status400BadRequest,
            "Attendance.OperationCancelled" => StatusCodes.Status409Conflict,
            "Attendance.DatabaseError"      => StatusCodes.Status500InternalServerError,
            "Attendance.InternalServerError"=> StatusCodes.Status500InternalServerError,
            _                               => StatusCodes.Status400BadRequest
        };

        return controller.Problem(
            statusCode: statusCode,
            title: code,
            detail: result.Error.Message);
    }
}