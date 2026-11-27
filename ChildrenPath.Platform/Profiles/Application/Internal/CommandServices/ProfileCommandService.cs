using ChildrenPath.Platform.Profiles.Application.CommandServices;
using ChildrenPath.Platform.Profiles.Domain.Model.Aggregates;
using ChildrenPath.Platform.Profiles.Domain.Model.Commands;
using ChildrenPath.Platform.Profiles.Domain.Model.Errors;
using ChildrenPath.Platform.Profiles.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Profiles.Application.Internal.CommandServices;

public class ProfileCommandService(IProfileRepository repository, IUnitOfWork unitOfWork)
    : IProfileCommandService
{
    public async Task<Result<Profile>> Handle(CreateProfileCommand command, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<UserRole>(command.Role, true, out var role))
            return Result<Profile>.Failure(ProfileErrors.InvalidRole);

        var profile = new Profile(command.UserId, command.FirstName, command.LastName, command.Phone, role);

        switch (role)
        {
            case UserRole.Parent:
                profile.SetParentProfile(command.Relationship ?? "Guardian");
                break;

            case UserRole.Driver:
                if (string.IsNullOrWhiteSpace(command.LicenseNumber) || command.LicenseExpiry is null)
                    return Result<Profile>.Failure(ProfileErrors.MissingRoleData);
                profile.SetDriverProfile(command.LicenseNumber, command.LicenseExpiry.Value);
                break;

            case UserRole.CompanyAdmin:
                if (string.IsNullOrWhiteSpace(command.TaxId) || string.IsNullOrWhiteSpace(command.LegalName))
                    return Result<Profile>.Failure(ProfileErrors.MissingRoleData);
                profile.SetCompanyProfile(command.TaxId, command.LegalName);
                break;
        }

        try
        {
            await repository.AddAsync(profile, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Profile>.Success(profile);
        }
        catch (DbUpdateException)
        {
            return Result<Profile>.Failure(ProfileErrors.UserIdAlreadyHasProfile);
        }
    }
}