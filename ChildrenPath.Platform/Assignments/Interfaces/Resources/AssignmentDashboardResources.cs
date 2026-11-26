namespace ChildrenPath.Platform.Assignments.Interfaces.Resources;

/// <summary>
///     Aggregated dashboard payload consumed by the Assignments Vue frontend view.
/// </summary>
public record AssignmentDashboardResource(
    AssignmentDashboardSummaryResource Summary,
    IEnumerable<AssignmentDashboardRecordResource> Assignments,
    IEnumerable<AssignmentDashboardReviewResource> Reviews,
    IEnumerable<AssignmentDashboardActivityResource> Activities);

/// <summary>
///     Summary KPI counters for the assignments dashboard.
/// </summary>
public record AssignmentDashboardSummaryResource(
    int TotalAssignments,
    int ValidatedAssignments,
    int PendingAssignments,
    int ConflictsDetected,
    int AverageCapacity,
    int AssignmentReadiness);

/// <summary>
///     Individual assignment record formatted for the Vue frontend cards, table, and dialogs.
/// </summary>
public record AssignmentDashboardRecordResource(
    string Id,
    string StudentCode,
    string StudentName,
    string Grade,
    string GuardianName,
    string RouteCode,
    string RouteName,
    string VehiclePlate,
    string DriverName,
    string Shift,
    string PickupPoint,
    string PickupWindow,
    int CapacityUsage,
    int ValidationScore,
    string Validation,
    string Status,
    string LastUpdated,
    string Notes);

/// <summary>
///     Review alert card displayed in the sidebar for pending or conflicted assignments.
/// </summary>
public record AssignmentDashboardReviewResource(
    string Id,
    string Title,
    string Description,
    string AssignmentCode,
    string Severity,
    string DueDate);

/// <summary>
///     Timeline activity entry displayed on the assignments dashboard.
/// </summary>
public record AssignmentDashboardActivityResource(
    string Id,
    string Time,
    string Title,
    string Description,
    string Status);