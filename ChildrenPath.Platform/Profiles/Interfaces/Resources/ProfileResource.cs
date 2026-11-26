namespace ChildrenPath.Platform.Profiles.Interfaces.Resources;

public record ProfileResource(
    int Id,
    int UserId,
    string FirstName,
    string LastName,
    string Phone,
    string Role
);