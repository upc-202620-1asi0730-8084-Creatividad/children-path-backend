using ChildrenPath.Platform.Iam.Domain.Model.Aggregates;
using ChildrenPath.Platform.Iam.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Iam.Interfaces.Rest.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(user.Id.Value, user.Username);
    }
}