using System.Globalization;
using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Domain.Model.ReadModels;
using ChildrenPath.Platform.Assignments.Interfaces.Resources;

namespace ChildrenPath.Platform.Assignments.Interfaces.Transform;

public static class AssignmentDashboardAssembler
{
    public static AssignmentDashboardResource ToDashboardResource(AssignmentDashboardSnapshot snapshot)
    {
        return new AssignmentDashboardResource(
            Summary: new AssignmentDashboardSummaryResource(
                TotalAssignments: snapshot.Summary.TotalAssignments,
                ValidatedAssignments: snapshot.Summary.ValidatedAssignments,
                PendingAssignments: snapshot.Summary.PendingAssignments,
                ConflictsDetected: snapshot.Summary.ConflictsDetected,
                AverageCapacity: snapshot.Summary.AverageCapacity,
                AssignmentReadiness: snapshot.Summary.AssignmentReadiness),
            Assignments: snapshot.Records.Select(ToDashboardRecordResource).ToList(),
            Reviews: snapshot.Reviews.Select(r => new AssignmentDashboardReviewResource(
                Id: $"AR-{r.AssignmentId:D3}",
                Title: r.Title,
                Description: r.Description,
                AssignmentCode: $"AS-{r.AssignmentId:D3} · {r.StudentName} · {r.LastUpdated:yyyy-MM-dd}",
                Severity: r.Severity,
                DueDate: r.LastUpdated.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))).ToList(),
            Activities: snapshot.Activities.Select(a => new AssignmentDashboardActivityResource(
                Id: $"AA-{a.AssignmentId:D3}",
                Time: $"{a.LastUpdated.ToString("h:mm tt", CultureInfo.InvariantCulture)} · {a.StudentName}",
                Title: a.Title,
                Description: a.Description,
                Status: a.StatusCategory)).ToList());
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
}