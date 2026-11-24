using ChildrenPath.Platform.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Attendance.Interfaces.Transform;

/// <summary>
///     Translates <see cref="Result{T}" /> outcomes from Attendance services into HTTP <see cref="IActionResult" /> responses.
/// </summary>
public static class AttendanceActionResultAssembler
{
    /// <summary>
    ///     Converts a <see cref="Result{T}" /> into a success <see cref="IActionResult" /> or a ProblemDetails error response.
    /// </summary>
    /// <typeparam name="T">The type of the result payload.</typeparam>
    /// <param name="controller">The controller handling the HTTP request.</param>
    /// <param name="result">The operation result to evaluate.</param>
    /// <param name="successAction">The callback producing the HTTP response when the result succeeds.</param>
    /// <returns>An <see cref="IActionResult" /> representing the HTTP outcome.</returns>
    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess)
            return successAction(result.Value!);

        var code = result.Message.Split(':')[0];

        var statusCode = code switch
        {
            "Attendance.NotFound" => StatusCodes.Status404NotFound,
            "Attendance.AlreadyExists" => StatusCodes.Status409Conflict,
            "Attendance.InvalidStatus" => StatusCodes.Status400BadRequest,
            "Attendance.OperationCancelled" => StatusCodes.Status409Conflict,
            "Attendance.DatabaseError" => StatusCodes.Status500InternalServerError,
            "Attendance.InternalServerError" => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };

        return controller.Problem(
            statusCode: statusCode,
            title: code,
            detail: result.Message);
    }
}