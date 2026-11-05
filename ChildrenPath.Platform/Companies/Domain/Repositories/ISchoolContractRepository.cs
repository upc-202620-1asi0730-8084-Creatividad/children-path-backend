using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Companies.Domain.Repositories;

public interface ISchoolContractRepository : IBaseRepository<SchoolContract>
{
    Task<IEnumerable<SchoolContract>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
}
