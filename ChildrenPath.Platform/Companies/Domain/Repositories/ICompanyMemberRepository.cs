using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Companies.Domain.Repositories;

public interface ICompanyMemberRepository : IBaseRepository<CompanyMember>
{
    Task<IEnumerable<CompanyMember>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}
