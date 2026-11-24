using ChildrenPath.Platform.Attendance.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Attendance.Interfaces.Resources;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Interfaces.Transform;

/// <summary>
///     Assembles frontend dashboard resources from collections of <see cref="AttendanceEntity" /> aggregates.
/// </summary>
public static class AttendanceDashboardAssembler
{
    /// <summary>
    ///     Builds a complete <see cref="AttendanceDashboardResource" /> from a collection of attendance aggregates.
    /// </summary>
    /// <param name="attendances">The attendance records retrieved from persistence.</param>
    /// <returns>An <see cref="AttendanceDashboardResource" /> ready for the frontend view.</returns>
    public static AttendanceDashboardResource ToDashboardResource(IEnumerable<AttendanceEntity> attendances)
    {
        var list = attendances.ToList();

        var records = list.Select(ToDashboardRecordResource).ToList();

        var totalAssigned = records.Count;
        var onBoard = records.Count(r => r.Status == "on_board");
        var arrived = records.Count(r => r.Status == "arrived");
        var waiting = records.Count(r => r.Status == "waiting");
        var absent = records.Count(r => r.Status == "absent");
        var pendingConfirmation = records.Count(r => r.Status == "pending_confirmation");

        var attended = onBoard + arrived;
        var denominator = Math.Max(totalAssigned - pendingConfirmation, 1);
        var attendanceReliability = totalAssigned == 0
            ? 0.0
            : Math.Round((double)attended / denominator * 1000.0) / 10.0;

        var summary = new AttendanceDashboardSummaryResource(
            TotalAssigned: totalAssigned,
            OnBoard: onBoard,
            Arrived: arrived,
            Waiting: waiting,
            Absent: absent,
            PendingConfirmation: pendingConfirmation,
            AttendanceReliability: attendanceReliability);

        var activities = list
            .OrderByDescending(a => a.Timestamp)
            .Take(10)
            .Select(a => new AttendanceDashboardActivityResource(
                Id: $"AA-{a.Id:D3}",
                Time: a.Timestamp.ToString("HH:mm"),
                Title: MapActivityTitle(a.Status),
                Description: MapActivityDescription(a),
                Status: a.Status is AttendanceStatus.Absent or AttendanceStatus.PendingConfirmation
                    ? "pending"
                    : a.Status == AttendanceStatus.Boarded
                        ? "active"
                        : "completed"))
            .ToList();

        return new AttendanceDashboardResource(summary, records, activities);
    }

    /// <summary>
    ///     Maps a single <see cref="AttendanceEntity" /> aggregate to an <see cref="AttendanceDashboardRecordResource" />.
    /// </summary>
    /// <param name="entity">The attendance aggregate to transform.</param>
    /// <returns>The dashboard-formatted attendance record.</returns>
    public static AttendanceDashboardRecordResource ToDashboardRecordResource(AttendanceEntity entity)
    {
        var frontendStatus = MapFrontendStatus(entity.Status);

        return new AttendanceDashboardRecordResource(
            Id: $"ATT-{entity.Id:D3}",
            StudentCode: $"ST-{entity.StudentId:D3}",
            StudentName: $"Student #{entity.StudentId}",
            Grade: "Assigned Grade",
            RouteName: $"Trip #{entity.TripId} Route",
            VehiclePlate: "PENDING",
            DriverName: "Assigned Driver",
            PickupPoint: entity.BoardingRecord?.Location ?? "Assigned Pickup Point",
            School: "Assigned School",
            EstimatedArrival: "07:30",
            CheckInTime: entity.BoardingRecord?.BoardedAt.ToString("HH:mm"),
            DropOffTime: entity.DroppedOffAt?.ToString("HH:mm"),
            Status: frontendStatus,
            Reliability: MapReliability(entity.Status),
            LastEvent: MapActivityTitle(entity.Status),
            Notes: MapNotes(entity),
            GuardianName: "Registered Guardian");
    }

    private static string MapFrontendStatus(AttendanceStatus status) => status switch
    {
        AttendanceStatus.Waiting => "waiting",
        AttendanceStatus.Boarded or AttendanceStatus.Late => "on_board",
        AttendanceStatus.DroppedOff => "arrived",
        AttendanceStatus.Absent => "absent",
        AttendanceStatus.PendingConfirmation => "pending_confirmation",
        _ => "waiting"
    };

    private static int MapReliability(AttendanceStatus status) => status switch
    {
        AttendanceStatus.DroppedOff => 100,
        AttendanceStatus.Boarded => 85,
        AttendanceStatus.Late => 70,
        AttendanceStatus.Absent => 0,
        _ => 50
    };

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

    private static string MapNotes(AttendanceEntity entity)
    {
        if (entity.Status == AttendanceStatus.Absent && entity.AbsenceRecord is not null)
            return entity.AbsenceRecord.Reason;

        if (entity.BoardingRecord is not null)
            return $"Boarded at {entity.BoardingRecord.Location}.";

        return "Awaiting status update.";
    }
}