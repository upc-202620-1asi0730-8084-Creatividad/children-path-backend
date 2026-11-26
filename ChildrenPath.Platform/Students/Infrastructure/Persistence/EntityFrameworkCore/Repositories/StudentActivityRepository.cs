using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using ChildrenPath.Platform.Students.Domain.Model.Entities;
using ChildrenPath.Platform.Students.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Students.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class StudentActivityRepository(AppDbContext context)
    : BaseRepository<StudentActivity>(context), IStudentActivityRepository
{
    public async Task<IEnumerable<StudentActivity>> ListRecentAsync(
        int limit,
        CancellationToken cancellationToken = default)
    {
        return await Context.Set<StudentActivity>()
            .AsNoTracking()
            .OrderByDescending(activity => activity.OccurredAt)
            .ThenByDescending(activity => activity.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}