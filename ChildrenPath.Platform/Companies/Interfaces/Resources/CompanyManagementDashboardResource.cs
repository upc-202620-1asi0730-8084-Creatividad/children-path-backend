namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

public record CompanyManagementDashboardResource(
    CompanyProfileResource Profile,
    CompanySummaryResource Summary,
    IEnumerable<SchoolContractResource> Contracts,
    IEnumerable<CompanyMemberResource> Members,
    IEnumerable<ComplianceItemResource> ComplianceItems,
    IEnumerable<CompanyActivityResource> Activities
);
