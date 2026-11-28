using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;

public static class SchoolContractResourceFromEntityAssembler
{
    public static SchoolContractResource ToResourceFromEntity(SchoolContract entity)
    {
        return new SchoolContractResource(
            entity.Id,
            entity.CompanyId,
            entity.SchoolName,
            entity.District,
            entity.RouteCount,
            entity.StudentCount,
            entity.ContactName,
            entity.RenewalDate,
            entity.Status,
            entity.Score,
            entity.Notes
        );
    }
}
