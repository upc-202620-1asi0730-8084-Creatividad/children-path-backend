using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;

public static class ComplianceItemResourceFromEntityAssembler
{
    public static ComplianceItemResource ToResourceFromEntity(ComplianceItem entity)
    {
        return new ComplianceItemResource(
            entity.Id,
            entity.CompanyId,
            entity.Title,
            entity.Category,
            entity.Owner,
            entity.DueDate,
            entity.Status,
            entity.Severity,
            entity.Description
        );
    }
}
