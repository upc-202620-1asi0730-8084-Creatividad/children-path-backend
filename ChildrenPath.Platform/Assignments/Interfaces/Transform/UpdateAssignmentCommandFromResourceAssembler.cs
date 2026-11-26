using ChildrenPath.Platform.Assignments.Domain.Model.Commands;
using ChildrenPath.Platform.Assignments.Interfaces.Resources;

namespace ChildrenPath.Platform.Assignments.Interfaces.Transform;

/// <summary>
///     Assembles <see cref="UpdateAssignmentCommand" /> instances from <see cref="UpdateAssignmentResource" /> payloads.
/// </summary>
public static class UpdateAssignmentCommandFromResourceAssembler
{
    public static UpdateAssignmentCommand ToCommandFromResource(int id, UpdateAssignmentResource resource)
    {
        return new UpdateAssignmentCommand(
            id,
            resource.StudentCode,
            resource.StudentName,
            resource.Grade,
            resource.GuardianName,
            resource.RouteCode,
            resource.RouteName,
            resource.VehiclePlate,
            resource.DriverName,
            AssignmentEnumMapper.ParseShift(resource.Shift),
            resource.PickupPoint,
            resource.PickupWindow,
            resource.CapacityUsage,
            resource.ValidationScore,
            AssignmentEnumMapper.ParseValidation(resource.Validation),
            AssignmentEnumMapper.ParseStatus(resource.Status),
            resource.Notes);
    }
}