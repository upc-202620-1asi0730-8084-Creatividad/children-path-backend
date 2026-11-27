using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Transform;

public static class CompanyManagementDashboardResourceAssembler
{
    public static CompanyManagementDashboardResource ToResourceFromData(CompanyManagementDashboardData data)
    {
        var profile = CompanyResourceFromEntityAssembler.ToProfileResourceFromEntity(data.Profile);
        var summary = new CompanySummaryResource(
            data.Summary.ActiveCompanies,
            data.Summary.TotalVehicles,
            data.Summary.ActiveDrivers,
            data.Summary.LinkedSchools,
            data.Summary.ManagedStudents,
            data.Summary.ComplianceScore,
            data.Summary.ServiceQualityScore,
            data.Summary.PendingReviews
        );
        var contracts = data.Contracts.Select(SchoolContractResourceFromEntityAssembler.ToResourceFromEntity);
        var members = data.Members.Select(CompanyMemberResourceFromEntityAssembler.ToResourceFromEntity);
        var complianceItems = data.ComplianceItems.Select(ComplianceItemResourceFromEntityAssembler.ToResourceFromEntity);
        var activities = data.Activities.Select(CompanyActivityResourceFromEntityAssembler.ToResourceFromEntity);

        return new CompanyManagementDashboardResource(
            profile,
            summary,
            contracts,
            members,
            complianceItems,
            activities
        );
    }
}
