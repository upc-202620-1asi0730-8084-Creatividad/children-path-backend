using AttendanceEntity = ChildrenPath.Platform.Attendance.Domain.Model.Aggregate.Attendance;

namespace ChildrenPath.Platform.Attendance.Domain.Model.ReadModels;

public record AttendanceDashboardSnapshot(
    AttendanceDashboardSummaryData Summary,
    IReadOnlyList<AttendanceEntity> Records,
    IReadOnlyList<AttendanceDashboardActivityData> Activities);

public record AttendanceDashboardSummaryData(
    int TotalAssigned,
    int OnBoard,
    int Arrived,
    int Waiting,
    int Absent,
    int PendingConfirmation,
    double AttendanceReliability);

public record AttendanceDashboardActivityData(
    int AttendanceId,
    DateTime Timestamp,
    string Title,
    string Description,
    string Status);