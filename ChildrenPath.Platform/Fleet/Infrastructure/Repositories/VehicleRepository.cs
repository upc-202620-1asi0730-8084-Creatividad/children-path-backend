using ChildrenPath.Platform.Fleet.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Fleet.Infrastructure.Repositories;

public interface IVehicleRepository : IBaseRepository<Vehicle> { }

public class VehicleRepository(AppDbContext context) : IVehicleRepository
{
    public async Task AddAsync(Vehicle entity, CancellationToken cancellationToken = default)
        => await context.Vehicles.AddAsync(entity, cancellationToken);

    public async Task<Vehicle?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Vehicles.FindAsync([id], cancellationToken);

    public void Update(Vehicle entity) => context.Vehicles.Update(entity);

    public void Remove(Vehicle entity) => context.Vehicles.Remove(entity);

    public async Task<IEnumerable<Vehicle>> ListAsync(CancellationToken cancellationToken = default)
        => await context.Vehicles.ToListAsync(cancellationToken);
}