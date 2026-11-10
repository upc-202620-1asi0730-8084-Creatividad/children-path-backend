using ChildrenPath.Platform.Companies.Domain.Model.Aggregates;
using ChildrenPath.Platform.Companies.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Companies.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CompanyRepository(AppDbContext context) : BaseRepository<Company>(context), ICompanyRepository
{
    public async Task<Company?> FindByRucAsync(string ruc, CancellationToken cancellationToken = default)
    {
        return await Context.Companies.FirstOrDefaultAsync(c => c.Ruc == ruc, cancellationToken);
    }

    public async Task<bool> ExistsByRucAsync(string ruc, CancellationToken cancellationToken = default)
    {
        return await Context.Companies.AnyAsync(c => c.Ruc == ruc, cancellationToken);
    }

    public async Task<Company?> GetDefaultCompanyAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Companies.FirstOrDefaultAsync(c => c.Status == "active", cancellationToken)
            ?? await Context.Companies.FirstOrDefaultAsync(cancellationToken);
    }
}
