using ChildrenPath.Platform.Profiles.Application.QueryServices;
using ChildrenPath.Platform.Profiles.Domain.Model.Aggregates;
using ChildrenPath.Platform.Profiles.Domain.Model.Queries;
using ChildrenPath.Platform.Profiles.Domain.Repositories;

namespace ChildrenPath.Platform.Profiles.Application.Internal.QueryServices;

public class ProfileQueryService(IProfileRepository profileRepository) : IProfileQueryService
{
    public async Task<Profile?> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken)
        => await profileRepository.FindByIdAsync(query.ProfileId, cancellationToken);

    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query, CancellationToken cancellationToken)
        => await profileRepository.ListAsync(cancellationToken);
}