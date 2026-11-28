using ChildrenPath.Platform.Iam.Domain.Model.Commands;
using ChildrenPath.Platform.Iam.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Iam.Interfaces.Rest.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Username, resource.Password);
    }
}