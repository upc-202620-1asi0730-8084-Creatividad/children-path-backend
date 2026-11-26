using ChildrenPath.Platform.Assignments.Domain.Model.Commands;
using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Assignments.Interfaces.Resources;

namespace ChildrenPath.Platform.Assignments.Interfaces.Transform;

/// <summary>
///     Assembles <see cref="CreateAssignmentCommand" /> instances from <see cref="CreateAssignmentResource" /> payloads.
/// </summary>
public static class CreateAssignmentCommandFromResourceAssembler
{
    public static CreateAssignmentCommand ToCommandFromResource(CreateAssignmentResource resource)
    {
        var shift = AssignmentEnumMapper.ParseShift(resource.Shift, defaultToMorningWhenEmpty: true);

        AssignmentValidation? validation = string.IsNullOrWhiteSpace(resource.Validation)
            ? null
            : AssignmentEnumMapper.ParseValidation(resource.Validation);

        AssignmentStatus? status = string.IsNullOrWhiteSpace(resource.Status)
            ? null
            : AssignmentEnumMapper.ParseStatus(resource.Status);

        return new CreateAssignmentCommand(
            resource.StudentCode,
            resource.StudentName,
            resource.Grade,
            resource.GuardianName,
            resource.RouteCode,
            resource.RouteName,
            resource.VehiclePlate,
            resource.DriverName,
            shift,
            resource.PickupPoint,
            resource.PickupWindow,
            resource.CapacityUsage,
            resource.Notes,
            resource.ValidationScore,
            validation,
            status);
    }
}