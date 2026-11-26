using System.Globalization;
using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Assignments.Interfaces.Resources;

namespace ChildrenPath.Platform.Assignments.Interfaces.Transform;

/// <summary>
///     Assembles frontend dashboard resources from collections of <see cref="Assignment" /> aggregates.
/// </summary>
public static class AssignmentDashboardAssembler
{
    public static AssignmentDashboardResource ToDashboardResource(IEnumerable<Assignment> assignments)
    {
        var list = assignments.ToList();
        var records = list.Select(ToDashboardRecordResource).ToList();

        var totalAssignments = list.Count;
        var validatedAssignments = list.Count(a => a.Status == AssignmentStatus.Validated);
        var pendingAssignments = list.Count(a => a.Status == AssignmentStatus.Pending);
        var conflictsDetected = list.Count(a => a.Status == AssignmentStatus.Conflict);

        var averageCapacity = totalAssignments == 0
            ? 0
            : (int)Math.Round(list.Average(a => a.CapacityUsage));

        var assignmentReadiness = totalAssignments == 0
            ? 0
            : (int)Math.Round((double)validatedAssignments / totalAssignments * 100.0);

        var summary = new AssignmentDashboardSummaryResource(
            TotalAssignments: totalAssignments,
            ValidatedAssignments: validatedAssignments,
            PendingAssignments: pendingAssignments,
            ConflictsDetected: conflictsDetected,
            AverageCapacity: averageCapacity,
            AssignmentReadiness: assignmentReadiness);

        var reviews = list
            .Where(a =>
                a.Status is AssignmentStatus.Conflict or AssignmentStatus.Pending ||
                a.Validation is AssignmentValidation.CapacityRisk
                    or AssignmentValidation.ScheduleConflict
                    or AssignmentValidation.MissingRoute)
            .OrderByDescending(a => a.Status == AssignmentStatus.Conflict)
            .ThenByDescending(a => a.LastUpdated)
            .Select(a => new AssignmentDashboardReviewResource(
                Id: $"AR-{a.Id:D3}",
                Title: MapReviewTitle(a),
                Description: !string.IsNullOrWhiteSpace(a.Notes)
                    ? a.Notes
                    : $"{a.StudentName} requires assignment verification for {a.RouteName}.",
                AssignmentCode: $"AS-{a.Id:D3} · {a.StudentName} · {a.LastUpdated:yyyy-MM-dd}",
                Severity: MapReviewSeverity(a),
                DueDate: a.LastUpdated.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)))
            .ToList();

        var activities = list
            .OrderByDescending(a => a.LastUpdated)
            .Take(10)
            .Select(a => new AssignmentDashboardActivityResource(
                Id: $"AA-{a.Id:D3}",
                Time: $"{a.LastUpdated.ToString("h:mm tt", CultureInfo.InvariantCulture)} · {a.StudentName}",
                Title: MapActivityTitle(a.Status),
                Description: MapActivityDescription(a),
                Status: a.Status switch
                {
                    AssignmentStatus.Validated => "completed",
                    AssignmentStatus.Pending => "active",
                    _ => "pending"
                }))
            .ToList();

        return new AssignmentDashboardResource(summary, records, reviews, activities);
    }

    public static AssignmentDashboardRecordResource ToDashboardRecordResource(Assignment entity)
    {
        return new AssignmentDashboardRecordResource(
            Id: $"AS-{entity.Id:D3}",
            StudentCode: entity.StudentCode,
            StudentName: entity.StudentName,
            Grade: entity.Grade,
            GuardianName: entity.GuardianName,
            RouteCode: entity.RouteCode,
            RouteName: entity.RouteName,
            VehiclePlate: entity.VehiclePlate,
            DriverName: entity.DriverName,
            Shift: AssignmentEnumMapper.ToFrontendShift(entity.Shift),
            PickupPoint: entity.PickupPoint,
            PickupWindow: entity.PickupWindow,
            CapacityUsage: entity.CapacityUsage,
            ValidationScore: entity.ValidationScore,
            Validation: AssignmentEnumMapper.ToFrontendValidation(entity.Validation),
            Status: AssignmentEnumMapper.ToFrontendStatus(entity.Status),
            LastUpdated: FormatLastUpdated(entity.LastUpdated),
            Notes: entity.Notes);
    }

    private static string FormatLastUpdated(DateTime lastUpdated)
    {
        var time = lastUpdated.ToString("h:mm tt", CultureInfo.InvariantCulture);
        return lastUpdated.Date == DateTime.UtcNow.Date
            ? $"Today, {time}"
            : $"{lastUpdated.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}, {time}";
    }

    private static string MapReviewTitle(Assignment entity) => entity.Validation switch
    {
        AssignmentValidation.ScheduleConflict => "Schedule conflict detected",
        AssignmentValidation.CapacityRisk => "Vehicle capacity risk",
        AssignmentValidation.MissingRoute => "Missing route assignment",
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
        AssignmentStatus.Pending => "Assignment pending review",
        AssignmentStatus.Conflict => "Conflict flagged",
        AssignmentStatus.Inactive => "Assignment deactivated",
        _ => "Assignment updated"
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