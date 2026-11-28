using ChildrenPath.Platform.Companies.Domain.Model.Commands;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;

public static class CreateCompanyCommandFromResourceAssembler
{
    public static CreateCompanyCommand ToCommandFromResource(CreateCompanyResource resource)
    {
        var districts = resource.OperatingDistricts != null
            ? string.Join(", ", resource.OperatingDistricts)
            : string.Empty;

        return new CreateCompanyCommand(
            resource.LegalName,
            resource.CommercialName,
            resource.Ruc,
            resource.Status ?? "active",
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
