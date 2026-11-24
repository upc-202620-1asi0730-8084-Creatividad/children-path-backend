namespace ChildrenPath.Platform.Attendance.Interfaces.Resources;

/// <summary>
///     Aggregated dashboard payload consumed by the Attendance frontend view.
/// </summary>
/// <param name="Summary">The aggregated attendance KPI metrics.</param>
/// <param name="Records">The list of attendance records formatted for the dashboard.</param>
/// <param name="Activities">The timeline of recent attendance status activities.</param>
public record AttendanceDashboardResource(
    AttendanceDashboardSummaryResource Summary,
    IEnumerable<AttendanceDashboardRecordResource> Records,
    IEnumerable<AttendanceDashboardActivityResource> Activities);

/// <summary>
///     Summary KPI counters for the attendance dashboard.
/// </summary>
/// <param name="TotalAssigned">Total number of assigned student attendance records.</param>
/// <param name="OnBoard">Number of students currently on board.</param>
/// <param name="Arrived">Number of students who have arrived at school.</param>
/// <param name="Waiting">Number of students waiting for pickup.</param>
/// <param name="Absent">Number of absent students.</param>
/// <param name="PendingConfirmation">Number of records pending confirmation.</param>
/// <param name="AttendanceReliability">Calculated attendance reliability percentage.</param>
public record AttendanceDashboardSummaryResource(
    int TotalAssigned,
    int OnBoard,
    int Arrived,
    int Waiting,
    int Absent,
    int PendingConfirmation,
    double AttendanceReliability);

/// <summary>
///     Individual attendance card item for the frontend dashboard.
/// </summary>
/// <param name="Id">Formatted attendance identifier (e.g., ATT-001).</param>
/// <param name="StudentCode">Formatted student code (e.g., ST-001).</param>
/// <param name="StudentName">Display name of the student.</param>
/// <param name="Grade">Academic grade label of the student.</param>
/// <param name="RouteName">Name of the assigned route.</param>
/// <param name="VehiclePlate">License plate of the assigned vehicle.</param>
/// <param name="DriverName">Display name of the assigned driver.</param>
/// <param name="PickupPoint">Pickup location associated with the record.</param>
/// <param name="School">Destination school name.</param>
/// <param name="EstimatedArrival">Estimated arrival time label.</param>
/// <param name="CheckInTime">Formatted boarding time, if recorded.</param>
/// <param name="DropOffTime">Formatted drop-off time, if recorded.</param>
/// <param name="Status">Frontend status key (waiting, on_board, arrived, absent, pending_confirmation).</param>
/// <param name="Reliability">Per-record reliability score (0-100).</param>
/// <param name="LastEvent">Title of the most recent status event.</param>
/// <param name="Notes">Additional notes or absence reason.</param>
/// <param name="GuardianName">Name of the student's guardian.</param>
public record AttendanceDashboardRecordResource(
    string Id,
    string StudentCode,
    string StudentName,
    string Grade,
    string RouteName,
    string VehiclePlate,
    string DriverName,
    string PickupPoint,
    string School,
    string EstimatedArrival,
    string? CheckInTime,
    string? DropOffTime,
    string Status,
    int Reliability,
    string LastEvent,
    string Notes,
    string GuardianName);

/// <summary>
///     Timeline activity entry displayed on the attendance dashboard.
/// </summary>
/// <param name="Id">Unique activity identifier.</param>
/// <param name="Time">Formatted event time.</param>
/// <param name="Title">Short title describing the event.</param>
/// <param name="Description">Detailed description of the event.</param>
/// <param name="Status">Visual status indicator (completed, active, pending).</param>
public record AttendanceDashboardActivityResource(
    string Id,
    string Time,
    string Title,
    string Description,
    string Status);