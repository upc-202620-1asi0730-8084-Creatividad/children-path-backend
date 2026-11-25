using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;

public partial class Assignment
{
    /// <summary>
    ///     Updates all mutable details of the assignment.
    /// </summary>
    public void UpdateDetails(
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
        int validationScore,
        AssignmentValidation validation,
        AssignmentStatus status,
        string notes)
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
        ValidationScore = Math.Clamp(validationScore, 0, 100);
        Validation = validation;
        Status = status;
        Notes = notes?.Trim() ?? string.Empty;
        LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    ///     Marks the assignment as validated and ready for operation.
    /// </summary>
    public void ValidateAssignment()
    {
        Status = AssignmentStatus.Validated;
        Validation = AssignmentValidation.Ready;
        ValidationScore = Math.Max(ValidationScore, 95);
        LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    ///     Marks the assignment as having a conflict with a specific reason.
    /// </summary>
    public void MarkAsConflict(string reason, AssignmentValidation validation = AssignmentValidation.ScheduleConflict)
    {
        Status = AssignmentStatus.Conflict;
        Validation = validation;
        ValidationScore = Math.Min(ValidationScore, 75);
        Notes = reason.Trim();
        LastUpdated = DateTime.UtcNow;
    }

    /// <summary>
    ///     Deactivates the assignment.
    /// </summary>
    public void Deactivate()
    {
        Status = AssignmentStatus.Inactive;
        Validation = AssignmentValidation.Inactive;
        LastUpdated = DateTime.UtcNow;
    }
}