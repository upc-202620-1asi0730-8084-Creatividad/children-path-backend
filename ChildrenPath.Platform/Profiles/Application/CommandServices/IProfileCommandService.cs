using ChildrenPath.Platform.Profiles.Domain.Model.Aggregates;
using ChildrenPath.Platform.Profiles.Domain.Model.Commands;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Profiles.Application.CommandServices;

public interface IProfileCommandService
{
    Task<Result<Profile>> Handle(CreateProfileCommand command, CancellationToken cancellationToken);
}