using ChildrenPath.Platform.Profiles.Domain.Model.Aggregates;
using ChildrenPath.Platform.Profiles.Domain.Model.Queries;

namespace ChildrenPath.Platform.Profiles.Application.QueryServices;

public interface IProfileQueryService
{
    Task<Profile?> Handle(GetProfileByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query, CancellationToken cancellationToken);
}