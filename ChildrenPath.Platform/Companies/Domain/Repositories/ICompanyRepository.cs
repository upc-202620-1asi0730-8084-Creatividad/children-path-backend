using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;
using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Companies.Domain.Repositories;

public interface ICompanyRepository : IBaseRepository<Company>
{
    Task<Company?> FindByRucAsync(string ruc, CancellationToken cancellationToken = default);
    Task<bool> ExistsByRucAsync(string ruc, CancellationToken cancellationToken = default);
    Task<Company?> GetDefaultCompanyAsync(CancellationToken cancellationToken = default);
}
