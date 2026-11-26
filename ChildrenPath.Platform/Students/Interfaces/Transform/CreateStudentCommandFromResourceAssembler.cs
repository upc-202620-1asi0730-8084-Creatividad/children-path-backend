using ChildrenPath.Platform.Students.Domain.Model.Commands;
using ChildrenPath.Platform.Students.Interfaces.Resources;

namespace ChildrenPath.Platform.Students.Interfaces.Transform;

public static class CreateStudentCommandFromResourceAssembler
{
    public static CreateStudentCommand ToCommandFromResource(CreateStudentResource resource)
    {
        return new CreateStudentCommand(
            resource.FirstName,
            resource.LastName,
            resource.Grade,
            resource.School,
            resource.GuardianName,
            resource.GuardianPhone,
            resource.GuardianEmail,
            resource.EmergencyContact,
            resource.RouteName,
            resource.PickupPoint,
            resource.PickupWindow,
            resource.Status);
    }
}