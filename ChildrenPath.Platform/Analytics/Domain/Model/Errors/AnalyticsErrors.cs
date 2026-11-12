using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Analytics.Domain.Model.Errors;

public static class AnalyticsErrors
{
    public static readonly Error ReportNotFound =
        new("Analytics.ReportNotFound", "The specified report was not found.");

    public static readonly Error ReportGenerationFailed =
        new("Analytics.ReportGenerationFailed", "An error occurred while generating the report.");

    public static readonly Error OperationCancelled =
        new("Analytics.OperationCancelled", "The operation was cancelled.");

    public static readonly Error DatabaseError =
        new("Analytics.DatabaseError", "A database error occurred.");

    public static readonly Error InternalServerError =
        new("Analytics.InternalServerError", "An internal server error occurred.");
}