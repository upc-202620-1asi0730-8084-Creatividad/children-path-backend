using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;
using ChildrenPath.Platform.Companies.Domain.Model.Commands;
using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Companies.Application.CommandServices;

public interface ICompanyCommandService
{
    Task<Result<Company>> Handle(CreateCompanyCommand command, CancellationToken cancellationToken = default);
    Task<Result<Company>> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken = default);
    Task<Result> HandleDelete(int id, CancellationToken cancellationToken = default);
    Task<Result<ComplianceItem>> Handle(UpdateComplianceItemStatusCommand command, CancellationToken cancellationToken = default);
    Task<Result<SchoolContract>> Handle(CreateSchoolContractCommand command, CancellationToken cancellationToken = default);
    Task<Result<CompanyMember>> Handle(CreateCompanyMemberCommand command, CancellationToken cancellationToken = default);
    Task<Result<ComplianceItem>> Handle(CreateComplianceItemCommand command, CancellationToken cancellationToken = default);
    Task<Result<CompanyActivity>> Handle(CreateCompanyActivityCommand command, CancellationToken cancellationToken = default);
    Task SeedInitialDataAsync(CancellationToken cancellationToken = default);
}