using ChildrenPath.Platform.Assignments.Application.QueryServices;
using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Domain.Model.Queries;
using ChildrenPath.Platform.Assignments.Domain.Model.ReadModels;
using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Assignments.Application.Internal.QueryServices;

public class AssignmentDashboardQueryService(IAssignmentQueryService assignmentQueryService)
    : IAssignmentDashboardQueryService
{
    public async Task<Result<AssignmentDashboardSnapshot>> Handle(
        GetAssignmentDashboardQuery query,
        CancellationToken cancellationToken)
    {
        var assignments = (await assignmentQueryService
            .Handle(new GetAllAssignmentsQuery(), cancellationToken)).ToList();

        var summary = BuildSummary(assignments);
        var reviews = BuildReviews(assignments);
        var activities = BuildActivities(assignments);

        return Result<AssignmentDashboardSnapshot>.Success(
            new AssignmentDashboardSnapshot(summary, assignments, reviews, activities));
    }

    private static AssignmentDashboardSummaryData BuildSummary(IReadOnlyList<Assignment> list)
    {
        var total = list.Count;
        var validated = list.Count(a => a.Status == AssignmentStatus.Validated);
        var pending = list.Count(a => a.Status == AssignmentStatus.Pending);
        var conflicts = list.Count(a => a.Status == AssignmentStatus.Conflict);

        var averageCapacity = total == 0
            ? 0
            : (int)Math.Round(list.Average(a => a.CapacityUsage));

        var readiness = total == 0
            ? 0
            : (int)Math.Round((double)validated / total * 100.0);

        return new AssignmentDashboardSummaryData(total, validated, pending, conflicts, averageCapacity, readiness);
    }

    private static IReadOnlyList<AssignmentDashboardReviewData> BuildReviews(IReadOnlyList<Assignment> list)
        => list
            .Where(a =>
                a.Status is AssignmentStatus.Conflict or AssignmentStatus.Pending ||
                a.Validation is AssignmentValidation.CapacityRisk
                    or AssignmentValidation.ScheduleConflict
                    or AssignmentValidation.MissingRoute)
            .OrderByDescending(a => a.Status == AssignmentStatus.Conflict)
            .ThenByDescending(a => a.LastUpdated)
            .Select(a => new AssignmentDashboardReviewData(
                AssignmentId: a.Id,
                Title: MapReviewTitle(a),
                Description: !string.IsNullOrWhiteSpace(a.Notes)
                    ? a.Notes
                    : $"{a.StudentName} requires assignment verification for {a.RouteName}.",
                StudentName: a.StudentName,
                LastUpdated: a.LastUpdated,
                Severity: MapReviewSeverity(a)))
            .ToList();

    private static IReadOnlyList<AssignmentDashboardActivityData> BuildActivities(IReadOnlyList<Assignment> list)
        => list
            .OrderByDescending(a => a.LastUpdated)
            .Take(10)
            .Select(a => new AssignmentDashboardActivityData(
                AssignmentId: a.Id,
                StudentName: a.StudentName,
                LastUpdated: a.LastUpdated,
                Title: MapActivityTitle(a.Status),
                Description: MapActivityDescription(a),
                StatusCategory: a.Status switch
                {
                    AssignmentStatus.Validated => "completed",
                    AssignmentStatus.Pending => "active",
                    _ => "pending"
                }))
            .ToList();

    private static string MapReviewTitle(Assignment entity) => entity.Validation switch
    {
        AssignmentValidation.ScheduleConflict => "Schedule conflict detected",
        AssignmentValidation.CapacityRisk     => "Vehicle capacity risk",
        AssignmentValidation.MissingRoute     => "Missing route assignment",
        _ => entity.Status == AssignmentStatus.Conflict
            ? "Assignment conflict detected"
            : "Assignment pending validation"
    };

    private static string MapReviewSeverity(Assignment entity)
    {
        if (entity.Status == AssignmentStatus.Conflict ||
            entity.Validation == AssignmentValidation.ScheduleConflict)
            return "high";

        if (entity.Validation is AssignmentValidation.CapacityRisk or AssignmentValidation.MissingRoute)
            return "medium";

        return "low";
    }

    private static string MapActivityTitle(AssignmentStatus status) => status switch
    {
        AssignmentStatus.Validated => "Assignment validated",
        AssignmentStatus.Pending   => "Assignment pending review",
        AssignmentStatus.Conflict  => "Conflict flagged",
        AssignmentStatus.Inactive  => "Assignment deactivated",
        _                          => "Assignment updated"
    };

    private static string MapActivityDescription(Assignment entity) => entity.Status switch
    {
        AssignmentStatus.Validated =>
            $"{entity.StudentName} was assigned to {entity.RouteName} with vehicle {entity.VehiclePlate}.",
        AssignmentStatus.Conflict =>
            !string.IsNullOrWhiteSpace(entity.Notes)
                ? entity.Notes
                : $"Conflict detected for {entity.StudentName} on {entity.RouteName}.",
        AssignmentStatus.Inactive =>
            $"Assignment for {entity.StudentName} on {entity.RouteName} was marked inactive.",
        _ =>
            $"{entity.StudentName} is awaiting validation on {entity.RouteName} ({entity.PickupWindow})."
    };
}