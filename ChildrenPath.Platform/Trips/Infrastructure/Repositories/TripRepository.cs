using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Trips.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Trips.Infrastructure.Repositories;

public interface ITripRepository : IBaseRepository<Trip> { }

public class TripRepository(AppDbContext context) : ITripRepository
{
    public async Task AddAsync(Trip entity, CancellationToken cancellationToken = default)
        => await context.Trips.AddAsync(entity, cancellationToken);

    public async Task<Trip?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Trips.FindAsync([id], cancellationToken);

    public void Update(Trip entity) => context.Trips.Update(entity);

    public void Remove(Trip entity) => context.Trips.Remove(entity);

    public async Task<IEnumerable<Trip>> ListAsync(CancellationToken cancellationToken = default)
        => await context.Trips.ToListAsync(cancellationToken);
}