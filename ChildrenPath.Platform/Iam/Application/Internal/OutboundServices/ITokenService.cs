using ChildrenPath.Platform.Iam.Domain.Model.Aggregates;

namespace ChildrenPath.Platform.Iam.Application.Internal.OutboundServices;

public interface ITokenService
{
    string GenerateToken(User user);
    Task<int?> ValidateToken(string token);
}