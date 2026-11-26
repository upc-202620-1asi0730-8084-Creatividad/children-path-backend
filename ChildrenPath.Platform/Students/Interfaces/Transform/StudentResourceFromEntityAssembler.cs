using ChildrenPath.Platform.Students.Domain.Model.Aggregates;
using ChildrenPath.Platform.Students.Interfaces.Resources;

namespace ChildrenPath.Platform.Students.Interfaces.Transform;

public static class StudentResourceFromEntityAssembler
{
    public static StudentResource ToResourceFromEntity(Student student)
    {
        var guardian = student.Guardians.OrderBy(item => item.Id).FirstOrDefault();
        return new StudentResource(
            student.Id,
            student.Code,
            student.FirstName,
            student.LastName,
            student.Grade,
            student.SchoolName,
            guardian?.FullName ?? string.Empty,
            guardian?.Phone ?? string.Empty,
            guardian?.Email,
            student.EmergencyContact,
            student.RouteName,
            student.RouteCode,
            student.AssignedVehicle,
            student.AssignedDriver,
            student.PickupPoint?.Address,
            student.DropOffPoint,
            student.PickupWindow,
            student.AuthorizationStatus,
            student.AttendanceRate,
            student.LastAttendanceStatus,
            student.Status,
            student.Notes);
    }
}