using ChildrenPath.Platform.Iam.Domain.Model.Commands;
using ChildrenPath.Platform.Iam.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Iam.Interfaces.Rest.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(resource.Username, resource.Password);
    }
}