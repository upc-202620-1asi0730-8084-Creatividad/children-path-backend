using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Fleet.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VehicleDocumentRepository(AppDbContext context) : BaseRepository<VehicleDocument>(context), IVehicleDocumentRepository
{
    public async Task<IEnumerable<VehicleDocument>> FindByVehicleIdAsync(int vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.VehicleDocuments
            .Where(d => d.VehicleId == vehicleId)
            .ToListAsync(cancellationToken);
    }
}
