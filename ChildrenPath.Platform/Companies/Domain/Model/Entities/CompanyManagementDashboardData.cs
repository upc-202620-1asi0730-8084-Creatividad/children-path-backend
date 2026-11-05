using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;

namespace ChildrenPath.Platform.Companies.Domain.Model.Entities;

public record CompanyManagementDashboardData(
    Company Profile,
    CompanySummary Summary,
    IEnumerable<SchoolContract> Contracts,
    IEnumerable<CompanyMember> Members,
    IEnumerable<ComplianceItem> ComplianceItems,
    IEnumerable<CompanyActivity> Activities
);
