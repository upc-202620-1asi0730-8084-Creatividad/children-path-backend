namespace ChildrenPath.Platform.Students.Interfaces.Resources;

public record CreateStudentResource(
    string FirstName,
    string LastName,
    string Grade,
    string SchoolName,
    string GuardianFullName,
    string GuardianPhone,
    string GuardianRelationship,
    string PickupAddress
);