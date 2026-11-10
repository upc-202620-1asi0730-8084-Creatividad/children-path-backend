using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Companies.Domain.Repositories;

public interface ICompanyActivityRepository : IBaseRepository<CompanyActivity>
{
    Task<IEnumerable<CompanyActivity>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}
