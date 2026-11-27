using ChildrenPath.Platform.Iam.Domain.Model.Aggregates;
using ChildrenPath.Platform.Iam.Domain.Model.Queries;

namespace ChildrenPath.Platform.Iam.Application.QueryServices;

public interface IUserQueryService
{
    Task<User?> Handle(GetUserByIdQuery query, CancellationToken cancellationToken);
}