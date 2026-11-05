using ChildrenPath.Platform.Companies.Domain.Model.Entities;
using ChildrenPath.Platform.Companies.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Companies.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SchoolContractRepository(AppDbContext context) : BaseRepository<SchoolContract>(context), ISchoolContractRepository
{
    public async Task<IEnumerable<SchoolContract>> FindByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await Context.SchoolContracts
            .Where(sc => sc.CompanyId == companyId)
            .ToListAsync(cancellationToken);
    }
}
