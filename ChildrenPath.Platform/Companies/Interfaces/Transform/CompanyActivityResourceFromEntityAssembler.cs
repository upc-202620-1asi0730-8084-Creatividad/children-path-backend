using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;

public static class CompanyActivityResourceFromEntityAssembler
{
    public static CompanyActivityResource ToResourceFromEntity(CompanyActivity entity)
    {
        return new CompanyActivityResource(
            entity.Id,
            entity.CompanyId,
            entity.Time,
            entity.Title,
            entity.Description,
            entity.Status
        );
    }
}
