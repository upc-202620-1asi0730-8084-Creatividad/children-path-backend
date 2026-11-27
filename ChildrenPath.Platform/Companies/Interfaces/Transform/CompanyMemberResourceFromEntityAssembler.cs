using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;

public static class CompanyMemberResourceFromEntityAssembler
{
    public static CompanyMemberResource ToResourceFromEntity(CompanyMember entity)
    {
        return new CompanyMemberResource(
            entity.Id,
            entity.CompanyId,
            entity.Name,
            entity.Email,
            entity.Role,
            entity.Status,
            entity.LastAccess,
            entity.Scope
        );
    }
}
