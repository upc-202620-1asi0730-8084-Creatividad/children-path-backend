using ChildrenPath.Platform.Profiles.Domain.Model.Aggregates;
using ChildrenPath.Platform.Profiles.Interfaces.Resources;

namespace ChildrenPath.Platform.Profiles.Interfaces.Transform;

public static class ProfileResourceFromEntityAssembler
{
    public static ProfileResource ToResourceFromEntity(Profile entity) => new(
        Id: (int)entity.Id,
        UserId: entity.UserId,
        FirstName: entity.FirstName,
        LastName: entity.LastName,
        Phone: entity.Phone,
        Role: entity.Role.ToString());
}