using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Dashboard.Domain.Model.Errors;

public static class DashboardErrors
{
    public static readonly Error InvalidRole =
        Error.Validation("Dashboard.InvalidRole", "The provided role is not a valid dashboard role.");

    public static readonly Error SnapshotGenerationFailed =
        Error.Unexpected("Dashboard.SnapshotGenerationFailed", "Failed to build the dashboard snapshot.");
}