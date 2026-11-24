using ChildrenPath.Platform.Fleet.Domain.Model.Aggregates;
using ChildrenPath.Platform.Fleet.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VehicleRepository(AppDbContext context) : BaseRepository<Vehicle>(context), IVehicleRepository
{
    public async Task<Vehicle?> FindByPlateAsync(string plate, CancellationToken cancellationToken = default)
    {
        return await Context.Vehicles.FirstOrDefaultAsync(v => v.Plate == plate, cancellationToken);
    }

    public async Task<Vehicle?> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await Context.Vehicles.FirstOrDefaultAsync(v => v.Code == code, cancellationToken);
    }

    public async Task<bool> ExistsByPlateAsync(string plate, CancellationToken cancellationToken = default)
    {
        return await Context.Vehicles.AnyAsync(v => v.Plate == plate, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await Context.Vehicles.AnyAsync(v => v.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> FindByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await Context.Vehicles
            .Where(v => v.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Vehicle>> FindByFleetIdAsync(int fleetId, CancellationToken cancellationToken = default)
    {
        return await Context.Vehicles
            .Where(v => v.FleetId == fleetId)
            .ToListAsync(cancellationToken);
    }
}
