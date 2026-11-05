using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;
using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Domain.Model.Queries;

namespace ChildrenPath.Platform.Companies.Application.QueryServices;

public interface ICompanyQueryService
{
    Task<IEnumerable<Company>> Handle(GetAllCompaniesQuery query, CancellationToken cancellationToken = default);
    Task<Company?> Handle(GetCompanyByIdQuery query, CancellationToken cancellationToken = default);
    Task<CompanyManagementDashboardData?> Handle(GetCompanyManagementDashboardQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<SchoolContract>> Handle(GetContractsByCompanyIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyMember>> Handle(GetMembersByCompanyIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<ComplianceItem>> Handle(GetComplianceItemsByCompanyIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<CompanyActivity>> Handle(GetActivitiesByCompanyIdQuery query, CancellationToken cancellationToken = default);
}
