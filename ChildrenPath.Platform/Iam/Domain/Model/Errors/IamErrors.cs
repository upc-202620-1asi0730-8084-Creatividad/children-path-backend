using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Iam.Domain.Model.Errors;

public static class IamErrors
{
    public static readonly Error InvalidCredentials =
        Error.Unauthorized("Iam.InvalidCredentials", "Invalid username or password.");

    public static readonly Error UsernameAlreadyTaken =
        Error.Conflict("Iam.UsernameAlreadyTaken", "The specified username is already taken.");

    public static readonly Error UserNotFound =
        Error.NotFound("Iam.UserNotFound", "The specified user was not found.");
}