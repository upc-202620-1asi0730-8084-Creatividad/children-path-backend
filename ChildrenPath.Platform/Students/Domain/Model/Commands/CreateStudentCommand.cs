namespace ChildrenPath.Platform.Students.Domain.Model.Commands;

public record CreateStudentCommand(
    string FirstName,
    string LastName,
    string Grade,
    string School,
    string GuardianName,
    string GuardianPhone,
    string? GuardianEmail,
    string? EmergencyContact,
    string? RouteName,
    string? PickupPoint,
    string? PickupWindow,
    string Status);