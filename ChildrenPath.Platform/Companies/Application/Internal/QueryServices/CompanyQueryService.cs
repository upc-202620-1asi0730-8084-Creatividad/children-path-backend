using ChildrenPath.Platform.Companies.Application.CommandServices;
using ChildrenPath.Platform.Companies.Application.QueryServices;
using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;
using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Domain.Model.Queries;
using ChildrenPath.Platform.Companies.Domain.Repositories;

namespace ChildrenPath.Platform.Companies.Application.Internal.QueryServices;

public class CompanyQueryService(
    ICompanyRepository companyRepository,
    ISchoolContractRepository schoolContractRepository,
    ICompanyMemberRepository companyMemberRepository,
    IComplianceItemRepository complianceItemRepository,
    ICompanyActivityRepository companyActivityRepository,
    ICompanyCommandService companyCommandService) : ICompanyQueryService
{
    public async Task<IEnumerable<Company>> Handle(GetAllCompaniesQuery query, CancellationToken cancellationToken = default)
    {
        var companies = await companyRepository.ListAsync(cancellationToken);
        var list = companies.ToList();
        if (list.Count == 0)
        {
            await companyCommandService.SeedInitialDataAsync(cancellationToken);
            list = (await companyRepository.ListAsync(cancellationToken)).ToList();
        }
        return list;
    }

    public async Task<Company?> Handle(GetCompanyByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await companyRepository.FindByIdAsync(query.Id, cancellationToken);
    }

    public async Task<CompanyManagementDashboardData?> Handle(GetCompanyManagementDashboardQuery query, CancellationToken cancellationToken = default)
    {
        Company? company = null;
        if (query.CompanyId.HasValue)
        {
            company = await companyRepository.FindByIdAsync(query.CompanyId.Value, cancellationToken);
        }
        else
        {
            company = await companyRepository.GetDefaultCompanyAsync(cancellationToken);
        }

        if (company == null)
        {
            await companyCommandService.SeedInitialDataAsync(cancellationToken);
            company = await companyRepository.GetDefaultCompanyAsync(cancellationToken);
        }

        if (company == null) return null;

        var contracts = (await schoolContractRepository.FindByCompanyIdAsync(company.Id, cancellationToken)).ToList();
        var members = (await companyMemberRepository.FindByCompanyIdAsync(company.Id, cancellationToken)).ToList();
        var complianceItems = (await complianceItemRepository.FindByCompanyIdAsync(company.Id, cancellationToken)).ToList();
        var activities = (await companyActivityRepository.FindByCompanyIdAsync(company.Id, cancellationToken)).ToList();

        var completedCompliance = complianceItems.Count(ci => ci.Status.Equals("completed", StringComparison.OrdinalIgnoreCase));
        var totalCompliance = complianceItems.Count;
        var complianceScore = totalCompliance > 0 ? (int)Math.Round((double)completedCompliance * 100 / totalCompliance) : 100;
        var serviceQualityScore = contracts.Count != 0 ? (int)Math.Round(contracts.Average(c => c.Score)) : 98;
        var pendingReviews = complianceItems.Count(ci => !ci.Status.Equals("completed", StringComparison.OrdinalIgnoreCase));
        var linkedSchools = contracts.Count;
        var managedStudents = contracts.Sum(c => c.StudentCount);
        var activeDrivers = members.Count(m => m.Role.Contains("conductor", StringComparison.OrdinalIgnoreCase) || m.Role.Contains("driver", StringComparison.OrdinalIgnoreCase));
        if (activeDrivers == 0 && members.Count != 0) activeDrivers = members.Count;

        var totalVehicles = company.PlanLimitVehicles > 0 ? company.PlanLimitVehicles : 24;

        var summary = new CompanySummary(
            ActiveCompanies: 1,
            TotalVehicles: totalVehicles,
            ActiveDrivers: activeDrivers > 0 ? activeDrivers : 18,
            LinkedSchools: linkedSchools > 0 ? linkedSchools : 12,
            ManagedStudents: managedStudents > 0 ? managedStudents : 1450,
            ComplianceScore: complianceScore,
            ServiceQualityScore: serviceQualityScore,
            PendingReviews: pendingReviews
        );

        return new CompanyManagementDashboardData(company, summary, contracts, members, complianceItems, activities);
    }

    public async Task<IEnumerable<SchoolContract>> Handle(GetContractsByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        return await schoolContractRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
    }

    public async Task<IEnumerable<CompanyMember>> Handle(GetMembersByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        return await companyMemberRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
    }

    public async Task<IEnumerable<ComplianceItem>> Handle(GetComplianceItemsByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        return await complianceItemRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
    }

    public async Task<IEnumerable<CompanyActivity>> Handle(GetActivitiesByCompanyIdQuery query, CancellationToken cancellationToken = default)
    {
        return await companyActivityRepository.FindByCompanyIdAsync(query.CompanyId, cancellationToken);
    }
}
