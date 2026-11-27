using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Errors;

public static class AlertErrors
{
    public static readonly Error AlertCreationFailed =
        Error.Failure("Alerts.AlertCreationFailed", "An error occurred while creating the alert.");

    public static readonly Error AlertNotFound =
        Error.NotFound("Alerts.AlertNotFound", "The specified alert was not found."); // <-- 404

    public static readonly Error AlertAlreadyResolved =
        Error.Conflict("Alerts.AlertAlreadyResolved", "The alert has already been resolved."); // <-- 409

    public static readonly Error OperationCancelled =
        Error.Conflict("Alerts.OperationCancelled", "The operation was cancelled.");

    public static readonly Error DatabaseError =
        Error.Unexpected("Alerts.DatabaseError", "A database error occurred while processing the request.");

    public static readonly Error InternalServerError =
        Error.Unexpected("Alerts.InternalServerError", "An internal server error occurred.");
}