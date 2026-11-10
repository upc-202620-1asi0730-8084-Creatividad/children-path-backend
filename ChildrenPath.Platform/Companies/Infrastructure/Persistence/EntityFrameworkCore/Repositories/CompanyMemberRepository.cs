using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Companies.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CompanyMemberRepository(AppDbContext context) : BaseRepository<CompanyMember>(context), ICompanyMemberRepository
{
    public async Task<IEnumerable<CompanyMember>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await Context.CompanyMembers
            .Where(cm => cm.CompanyId == companyId)
            .ToListAsync(cancellationToken);
    }
}
