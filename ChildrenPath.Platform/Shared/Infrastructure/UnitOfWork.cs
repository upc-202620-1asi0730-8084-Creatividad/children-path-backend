using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;

namespace ChildrenPath.Platform.Infrastructure;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}