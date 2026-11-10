using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Companies.Domain.Repositories;

public interface IComplianceItemRepository : IBaseRepository<ComplianceItem>
{
    Task<IEnumerable<ComplianceItem>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}
