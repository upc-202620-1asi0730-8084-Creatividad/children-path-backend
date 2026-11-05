using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Companies.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ComplianceItemRepository(AppDbContext context) : BaseRepository<ComplianceItem>(context), IComplianceItemRepository
{
    public async Task<IEnumerable<ComplianceItem>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await Context.ComplianceItems
            .Where(ci => ci.CompanyId == companyId)
            .ToListAsync(cancellationToken);
    }
}
