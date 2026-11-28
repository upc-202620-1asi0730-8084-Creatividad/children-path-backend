using ChildrenPath.Platform.Companies.Domain.Model.Commands;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;

public static class UpdateComplianceItemStatusCommandFromResourceAssembler
{
    public static UpdateComplianceItemStatusCommand ToCommandFromResource(int id, UpdateComplianceStatusResource resource)
    {
        return new UpdateComplianceItemStatusCommand(id, resource.Status);
    }
}
