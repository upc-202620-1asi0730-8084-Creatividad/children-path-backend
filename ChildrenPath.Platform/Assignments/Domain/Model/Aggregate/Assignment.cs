using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;

/// <summary>
///     Aggregate root representing the assignment of a student to a route, vehicle, and driver for a specific shift.
/// </summary>
public partial class Assignment
{
    public int Id { get; private set; }

    // Student details
    public string StudentCode { get; private set; } = string.Empty;
    public string StudentName { get; private set; } = string.Empty;
    public string Grade { get; private set; } = string.Empty;
    public string GuardianName { get; private set; } = string.Empty;

    // Route & Vehicle details
    public string RouteCode { get; private set; } = string.Empty;
    public string RouteName { get; private set; } = string.Empty;
    public string VehiclePlate { get; private set; } = string.Empty;
    public string DriverName { get; private set; } = string.Empty;

    // Assignment scheduling
    public AssignmentShift Shift { get; private set; } = AssignmentShift.Morning;
    public string PickupPoint { get; private set; } = string.Empty;
    public string PickupWindow { get; private set; } = string.Empty;

    // Validation & Status
    public int CapacityUsage { get; private set; }
    public int ValidationScore { get; private set; }
    public AssignmentValidation Validation { get; private set; } = AssignmentValidation.Ready;
    public AssignmentStatus Status { get; private set; } = AssignmentStatus.Pending;

    public DateTime LastUpdated { get; private set; } = DateTime.UtcNow;
    public string Notes { get; private set; } = string.Empty;

    /// <summary>
    ///     Parameterless constructor required by Entity Framework Core.
    /// </summary>
    protected Assignment()
    {
    }

    /// <summary>
    ///     Initializes a new instance of <see cref="Assignment" />.
    /// </summary>
    public Assignment(
        string studentCode,
        string studentName,
        string grade,
        string guardianName,
        string routeCode,
        string routeName,
        string vehiclePlate,
        string driverName,
        AssignmentShift shift,
        string pickupPoint,
        string pickupWindow,
        int capacityUsage,
        string notes,
        int? validationScore = null,
        AssignmentValidation? validation = null,
        AssignmentStatus? status = null)
    {
        StudentCode = studentCode.Trim();
        StudentName = studentName.Trim();
        Grade = grade.Trim();
        GuardianName = guardianName.Trim();
        RouteCode = routeCode.Trim();
        RouteName = routeName.Trim();
        VehiclePlate = vehiclePlate.Trim();
        DriverName = driverName.Trim();
        Shift = shift;
        PickupPoint = pickupPoint.Trim();
        PickupWindow = pickupWindow.Trim();
        CapacityUsage = Math.Clamp(capacityUsage, 0, 100);
        ValidationScore = Math.Clamp(validationScore ?? 90, 0, 100);
        Validation = validation ?? AssignmentValidation.Ready;
        Status = status ?? AssignmentStatus.Pending;
        Notes = notes?.Trim() ?? string.Empty;
        LastUpdated = DateTime.UtcNow;
    }
}