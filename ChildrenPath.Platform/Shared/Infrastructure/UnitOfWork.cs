using ChildrenPath.Platform.Shared.Domain.Repositories;

namespace ChildrenPath.Platform.Shared.Infrastructure;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync(CancellationToken cancellationToken = default)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}