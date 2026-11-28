using ChildrenPath.Platform.Attendance.Domain.Model.ReadModels;
using ChildrenPath.Platform.Attendance.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Attendance.Interfaces.Resources;
using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Interfaces.Transform;

public static class AttendanceDashboardAssembler
{
    public static AttendanceDashboardResource ToDashboardResource(AttendanceDashboardSnapshot snapshot)
    {
        var records = snapshot.Records.Select(ToDashboardRecordResource).ToList();

        var summary = new AttendanceDashboardSummaryResource(
            TotalAssigned: snapshot.Summary.TotalAssigned,
            OnBoard: snapshot.Summary.OnBoard,
            Arrived: snapshot.Summary.Arrived,
            Waiting: snapshot.Summary.Waiting,
            Absent: snapshot.Summary.Absent,
            PendingConfirmation: snapshot.Summary.PendingConfirmation,
            AttendanceReliability: snapshot.Summary.AttendanceReliability
        );

        var activities = snapshot.Activities.Select(a => new AttendanceDashboardActivityResource(
            Id: $"AA-{a.AttendanceId:D3}",
            Time: a.Timestamp.ToString("HH:mm"),
            Title: a.Title,
            Description: a.Description,
            Status: a.Status
        )).ToList();

        return new AttendanceDashboardResource(summary, records, activities);
    }

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

    private static string MapNotes(AttendanceEntity entity)
    {
        if (entity.Status == AttendanceStatus.Absent && entity.AbsenceRecord is not null)
            return entity.AbsenceRecord.Reason;

        if (entity.BoardingRecord is not null)
            return $"Boarded at {entity.BoardingRecord.Location}.";

        return "Awaiting status update.";
    }
}