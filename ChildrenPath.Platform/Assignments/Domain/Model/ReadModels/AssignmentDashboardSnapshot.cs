using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;

namespace ChildrenPath.Platform.Assignments.Domain.Model.ReadModels;

public record AssignmentDashboardSnapshot(
    AssignmentDashboardSummaryData Summary,
    IReadOnlyList<Assignment> Records,
    IReadOnlyList<AssignmentDashboardReviewData> Reviews,
    IReadOnlyList<AssignmentDashboardActivityData> Activities);

public record AssignmentDashboardSummaryData(
    int TotalAssignments,
    int ValidatedAssignments,
    int PendingAssignments,
    int ConflictsDetected,
    int AverageCapacity,
    int AssignmentReadiness);

public record AssignmentDashboardReviewData(
    int AssignmentId,
    string Title,
    string Description,
    string StudentName,
    DateTime LastUpdated,
    string Severity);

public record AssignmentDashboardActivityData(
    int AssignmentId,
    string StudentName,
    DateTime LastUpdated,
    string Title,
    string Description,
    string StatusCategory);