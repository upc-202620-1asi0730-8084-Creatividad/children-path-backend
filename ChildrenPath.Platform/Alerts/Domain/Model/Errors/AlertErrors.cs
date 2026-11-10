using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Alerts.Domain.Model.Errors;

public static class AlertErrors
{
    public static readonly Error AlertCreationFailed =
        new("Alerts.AlertCreationFailed", "An error occurred while creating the alert.");

    public static readonly Error AlertNotFound =
        new("Alerts.AlertNotFound", "The specified alert was not found.");

    public static readonly Error AlertAlreadyResolved =
        new("Alerts.AlertAlreadyResolved", "The alert has already been resolved.");

    public static readonly Error OperationCancelled =
        new("Alerts.OperationCancelled", "The operation was cancelled.");

    public static readonly Error DatabaseError =
        new("Alerts.DatabaseError", "A database error occurred while processing the request.");

    public static readonly Error InternalServerError =
        new("Alerts.InternalServerError", "An internal server error occurred.");
}