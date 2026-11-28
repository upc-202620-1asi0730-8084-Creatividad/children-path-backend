using ChildrenPath.Platform.Attendance.Application.QueryServices;
using ChildrenPath.Platform.Attendance.Domain.Model.Queries;
using ChildrenPath.Platform.Attendance.Domain.Model.ReadModels;
using ChildrenPath.Platform.Attendance.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Application.Internal.QueryServices;

public class AttendanceDashboardQueryService(IAttendanceQueryService attendanceQueryService)
    : IAttendanceDashboardQueryService
{
    public async Task<Result<AttendanceDashboardSnapshot>> Handle(
        GetAttendanceDashboardQuery query,
        CancellationToken cancellationToken)
    {
        var attendances = (await attendanceQueryService.Handle(new GetAllAttendancesQuery(), cancellationToken)).ToList();

        var totalAssigned = attendances.Count;
        var onBoard = attendances.Count(a => a.Status == AttendanceStatus.Boarded || a.Status == AttendanceStatus.Late);
        var arrived = attendances.Count(a => a.Status == AttendanceStatus.DroppedOff);
        var waiting = attendances.Count(a => a.Status == AttendanceStatus.Waiting);
        var absent = attendances.Count(a => a.Status == AttendanceStatus.Absent);
        var pendingConfirmation = attendances.Count(a => a.Status == AttendanceStatus.PendingConfirmation);

        var attended = onBoard + arrived;
        var denominator = Math.Max(totalAssigned - pendingConfirmation, 1);
        var reliability = totalAssigned == 0 ? 0.0 : Math.Round((double)attended / denominator * 1000.0) / 10.0;

        var summary = new AttendanceDashboardSummaryData(
            totalAssigned, onBoard, arrived, waiting, absent, pendingConfirmation, reliability);

        var activities = attendances
            .OrderByDescending(a => a.Timestamp)
            .Take(10)
            .Select(a => new AttendanceDashboardActivityData(
                AttendanceId: a.Id,
                Timestamp: a.Timestamp,
                Title: MapActivityTitle(a.Status),
                Description: MapActivityDescription(a),
                Status: a.Status is AttendanceStatus.Absent or AttendanceStatus.PendingConfirmation
                    ? "pending"
                    : a.Status == AttendanceStatus.Boarded
                        ? "active"
                        : "completed"))
            .ToList();

        return Result<AttendanceDashboardSnapshot>.Success(
            new AttendanceDashboardSnapshot(summary, attendances, activities));
    }

    private static string MapActivityTitle(AttendanceStatus status) => status switch
    {
        AttendanceStatus.Waiting => "Student marked as waiting",
        AttendanceStatus.Boarded => "Boarding confirmed",
        AttendanceStatus.DroppedOff => "Drop-off confirmed",
        AttendanceStatus.Absent => "Absence registered",
        AttendanceStatus.Late => "Late boarding recorded",
        AttendanceStatus.PendingConfirmation => "Attendance review requested",
        _ => "Attendance updated"
    };

    private static string MapActivityDescription(AttendanceEntity entity) => entity.Status switch
    {
        AttendanceStatus.Boarded => $"Student ST-{entity.StudentId:D3} boarded at {entity.BoardingRecord?.Location ?? "pickup point"}.",
        AttendanceStatus.DroppedOff => $"Student ST-{entity.StudentId:D3} arrived at destination.",
        AttendanceStatus.Absent => $"Student ST-{entity.StudentId:D3} marked absent: {entity.AbsenceRecord?.Reason ?? "No reason provided"}.",
        AttendanceStatus.Late => $"Student ST-{entity.StudentId:D3} boarded late.",
        AttendanceStatus.PendingConfirmation => $"Student ST-{entity.StudentId:D3} requires attendance confirmation.",
        _ => $"Student ST-{entity.StudentId:D3} is waiting for pickup."
    };
}