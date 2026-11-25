using ChildrenPath.Platform.Iam.Domain.Model.Aggregates;
using ChildrenPath.Platform.Iam.Domain.Model.Commands;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Iam.Application.CommandServices;

public interface IUserCommandService
{
    Task<Result<(User user, string token)>> Handle(SignInCommand command, CancellationToken cancellationToken);
    Task<Result<User>> Handle(SignUpCommand command, CancellationToken cancellationToken);
}