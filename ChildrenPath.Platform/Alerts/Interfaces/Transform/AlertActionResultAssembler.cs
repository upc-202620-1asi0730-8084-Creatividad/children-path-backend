using ChildrenPath.Platform.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Alerts.Interfaces.Transform;

public static class AlertActionResultAssembler
{
    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess)
        {
            return successAction(result.Value!);
        }

        var code = result.Message.Split(':')[0];
        
        var statusCode = code switch
        {
            "Alerts.AlertNotFound" => StatusCodes.Status404NotFound,
            "Alerts.AlertAlreadyResolved" => StatusCodes.Status409Conflict,
            "Alerts.OperationCancelled" => StatusCodes.Status409Conflict,
            "Alerts.DatabaseError" => StatusCodes.Status500InternalServerError,
            "Alerts.InternalServerError" => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };

        return controller.Problem(
            statusCode: statusCode,
            title: code,
            detail: result.Message
        );
    }
}