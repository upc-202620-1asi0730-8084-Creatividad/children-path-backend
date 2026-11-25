using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;

public static class ResultActionResultAssembler
{
    public static IActionResult ToActionResult<T>(
        ControllerBase controller,
        Result<T> result,
        Func<T, IActionResult> successAction)
    {
        if (result.IsSuccess)
            return successAction(result.Value!);

        var statusCode = result.Error.Type switch
        {
            EErrorType.NotFound     => StatusCodes.Status404NotFound,
            EErrorType.Conflict     => StatusCodes.Status409Conflict,
            EErrorType.Validation   => StatusCodes.Status400BadRequest,
            EErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            EErrorType.Forbidden    => StatusCodes.Status403Forbidden,
            EErrorType.Unexpected   => StatusCodes.Status500InternalServerError,
            _                       => StatusCodes.Status400BadRequest
        };

        return controller.Problem(
            statusCode: statusCode,
            title: result.Error.Message,
            detail: result.Error.Message,
            instance: controller.HttpContext.Request.Path,
            extensions: new Dictionary<string, object?> { ["code"] = result.Error.Code });
    }
}