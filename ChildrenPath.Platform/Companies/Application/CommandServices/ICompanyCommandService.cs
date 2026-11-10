using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;
using ChildrenPath.Platform.Companies.Domain.Model.Commands;
using ChildrenPath.Platform.Companies.Domain.Model.Entities;

namespace ChildrenPath.Platform.Companies.Application.CommandServices;

public interface ICompanyCommandService
{
    Task<Company?> Handle(CreateCompanyCommand command, CancellationToken cancellationToken = default);
    Task<Company?> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken = default);
    Task<bool> HandleDelete(int id, CancellationToken cancellationToken = default);
    Task<ComplianceItem?> Handle(UpdateComplianceItemStatusCommand command, CancellationToken cancellationToken = default);
    Task<SchoolContract?> Handle(CreateSchoolContractCommand command, CancellationToken cancellationToken = default);
    Task<CompanyMember?> Handle(CreateCompanyMemberCommand command, CancellationToken cancellationToken = default);
    Task<ComplianceItem?> Handle(CreateComplianceItemCommand command, CancellationToken cancellationToken = default);
    Task<CompanyActivity?> Handle(CreateCompanyActivityCommand command, CancellationToken cancellationToken = default);
    Task SeedInitialDataAsync(CancellationToken cancellationToken = default);
}
