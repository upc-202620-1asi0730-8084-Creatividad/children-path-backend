using ChildrenPath.Platform.Shared.Domain;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Trips.Interfaces.Transform;

public static class TripActionResultAssembler
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
            "Trips.TripNotFound" => StatusCodes.Status404NotFound,
            "Trips.TripAlreadyCompleted" => StatusCodes.Status409Conflict,
            "Trips.TripAlreadyCancelled" => StatusCodes.Status409Conflict,
            "Trips.OperationCancelled" => StatusCodes.Status409Conflict,
            "Trips.DatabaseError" => StatusCodes.Status500InternalServerError,
            "Trips.InternalServerError" => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status400BadRequest
        };

        return controller.Problem(
            statusCode: statusCode,
            title: code,
            detail: result.Message
        );
    }
}