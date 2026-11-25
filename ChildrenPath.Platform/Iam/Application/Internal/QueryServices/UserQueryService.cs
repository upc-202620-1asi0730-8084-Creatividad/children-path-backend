using ChildrenPath.Platform.Iam.Application.QueryServices;
using ChildrenPath.Platform.Iam.Domain.Model.Aggregates;
using ChildrenPath.Platform.Iam.Domain.Model.Queries;
using ChildrenPath.Platform.Iam.Domain.Repositories;

namespace ChildrenPath.Platform.Iam.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public async Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        return await userRepository.FindByIdAsync(query.Id, cancellationToken);
    }
}