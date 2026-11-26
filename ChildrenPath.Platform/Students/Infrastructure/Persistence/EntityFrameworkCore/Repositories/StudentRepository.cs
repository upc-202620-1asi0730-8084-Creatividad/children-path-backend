using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using ChildrenPath.Platform.Students.Domain.Model.Aggregates;
using ChildrenPath.Platform.Students.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Students.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class StudentRepository(AppDbContext context) : BaseRepository<Student>(context), IStudentRepository
{
    public async Task<Student?> FindWithRelationsByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await Context.Students
            .Include(student => student.Guardians)
            .Include(student => student.PickupPoint)
            .FirstOrDefaultAsync(student => student.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Student>> ListWithRelationsAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Students
            .AsNoTracking()
            .Include(student => student.Guardians)
            .Include(student => student.PickupPoint)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetNextCodeNumberAsync(CancellationToken cancellationToken = default)
    {
        var codes = await Context.Students
            .AsNoTracking()
            .Select(student => student.Code)
            .ToListAsync(cancellationToken);

        return codes
            .Select(code => !string.IsNullOrEmpty(code) &&
                            code.StartsWith("ST-", StringComparison.OrdinalIgnoreCase) &&
                            int.TryParse(code.AsSpan(3), out var number)
                ? number
                : 0)
            .DefaultIfEmpty()
            .Max() + 1;
    }
}