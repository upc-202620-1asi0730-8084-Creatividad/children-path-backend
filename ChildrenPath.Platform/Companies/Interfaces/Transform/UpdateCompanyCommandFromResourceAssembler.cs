using ChildrenPath.Platform.Companies.Domain.Model.Commands;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;

public static class UpdateCompanyCommandFromResourceAssembler
{
    public static UpdateCompanyCommand ToCommandFromResource(int id, UpdateCompanyResource resource)
    {
        var districts = resource.OperatingDistricts != null
            ? string.Join(", ", resource.OperatingDistricts)
            : string.Empty;

        return new UpdateCompanyCommand(
            id,
            resource.LegalName,
            resource.CommercialName,
            resource.Ruc,
            resource.Status,
            resource.Plan,
            resource.PlanLimitVehicles,
            resource.PlanLimitStudents,
            resource.AdminName,
            resource.AdminEmail,
            resource.Phone,
            resource.Address,
            districts,
            resource.Verified,
            resource.LicenseExpiration
        );
    }
}
