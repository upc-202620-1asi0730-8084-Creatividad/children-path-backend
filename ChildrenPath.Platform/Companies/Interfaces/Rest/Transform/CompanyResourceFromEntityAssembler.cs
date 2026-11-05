using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;

public static class CompanyResourceFromEntityAssembler
{
    public static CompanyResource ToResourceFromEntity(Company entity)
    {
        var districts = string.IsNullOrWhiteSpace(entity.OperatingDistricts)
            ? []
            : entity.OperatingDistricts.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        return new CompanyResource(
            entity.Id,
            entity.LegalName,
            entity.CommercialName,
            entity.Ruc,
            entity.Status,
            entity.Plan,
            entity.PlanLimitVehicles,
            entity.PlanLimitStudents,
            entity.AdminName,
            entity.AdminEmail,
            entity.Phone,
            entity.Address,
            districts,
            entity.Verified,
            entity.LicenseExpiration
        );
    }

    public static CompanyProfileResource ToProfileResourceFromEntity(Company entity)
    {
        var districts = string.IsNullOrWhiteSpace(entity.OperatingDistricts)
            ? []
            : entity.OperatingDistricts.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        return new CompanyProfileResource(
            entity.Id,
            entity.LegalName,
            entity.CommercialName,
            entity.Ruc,
            entity.Status,
            entity.Plan,
            entity.PlanLimitVehicles,
            entity.PlanLimitStudents,
            entity.AdminName,
            entity.AdminEmail,
            entity.Phone,
            entity.Address,
            districts,
            entity.Verified,
            entity.LicenseExpiration
        );
    }
}
