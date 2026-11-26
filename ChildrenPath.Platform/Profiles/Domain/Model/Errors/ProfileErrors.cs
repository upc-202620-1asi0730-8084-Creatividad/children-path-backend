using ChildrenPath.Platform.Shared.Domain.Model;

namespace ChildrenPath.Platform.Profiles.Domain.Model.Errors;

public static class ProfileErrors
{
    public static readonly Error ProfileNotFound =
        Error.NotFound("Profiles.ProfileNotFound", "The specified profile was not found.");

    public static readonly Error InvalidRole =
        Error.Validation("Profiles.InvalidRole", "The specified role is invalid.");

    public static readonly Error MissingRoleData =
        Error.Validation("Profiles.MissingRoleData", "Required data for the selected role is missing.");

    public static readonly Error UserIdAlreadyHasProfile =
        Error.Conflict("Profiles.UserIdAlreadyHasProfile", "The user already has an associated profile.");

    public static readonly Error DatabaseError =
        Error.Unexpected("Profiles.DatabaseError", "A database error occurred while processing the request.");
}