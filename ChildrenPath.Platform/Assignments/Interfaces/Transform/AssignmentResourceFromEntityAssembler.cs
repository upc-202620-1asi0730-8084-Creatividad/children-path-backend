using ChildrenPath.Platform.Assignments.Domain.Model.Aggregate;
using ChildrenPath.Platform.Assignments.Interfaces.Resources;

namespace ChildrenPath.Platform.Assignments.Interfaces.Transform;

/// <summary>
///     Assembles <see cref="AssignmentResource" /> DTOs from <see cref="Assignment" /> aggregates.
/// </summary>
public static class AssignmentResourceFromEntityAssembler
{
    public static AssignmentResource ToResourceFromEntity(Assignment entity)
    {
        return new AssignmentResource(
            entity.Id,
            entity.StudentCode,
            entity.StudentName,
            entity.Grade,
            entity.GuardianName,
            entity.RouteCode,
            entity.RouteName,
            entity.VehiclePlate,
            entity.DriverName,
            entity.Shift.ToString(),
            entity.PickupPoint,
            entity.PickupWindow,
            entity.CapacityUsage,
            entity.ValidationScore,
            entity.Validation.ToString(),
            entity.Status.ToString(),
            entity.LastUpdated,
            entity.Notes);
    }
}