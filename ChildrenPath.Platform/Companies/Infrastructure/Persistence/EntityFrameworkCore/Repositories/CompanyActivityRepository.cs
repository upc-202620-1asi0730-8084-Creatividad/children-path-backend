using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Companies.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CompanyActivityRepository(AppDbContext context) : BaseRepository<CompanyActivity>(context), ICompanyActivityRepository
{
    public async Task<IEnumerable<CompanyActivity>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await Context.CompanyActivities
            .Where(ca => ca.CompanyId == companyId)
            .OrderByDescending(ca => ca.Time)
            .ToListAsync(cancellationToken);
    }
}
