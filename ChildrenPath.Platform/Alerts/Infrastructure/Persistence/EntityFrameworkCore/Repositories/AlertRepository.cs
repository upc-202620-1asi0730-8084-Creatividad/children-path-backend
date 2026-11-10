using ChildrenPath.Platform.Alerts.Domain.Model.Aggregate;
using ChildrenPath.Platform.Alerts.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Alerts.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure; 
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Alerts.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class AlertRepository(AppDbContext context)
    : BaseRepository<Alert>(context), IAlertRepository
{
    // Variable privada. Al usar _context, evitamos cualquier error de herencia.
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Alert>> FindByTripIdAsync(int tripId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Alert>()
            .Include(a => a.Notifications)
            .Where(a => a.TripId == tripId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Alert>> FindByStatusAsync(AlertStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Alert>()
            .Include(a => a.Notifications)
            .Where(a => a.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Alert>> FindAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Alert>()
            .Include(a => a.Notifications)
            .Where(a => a.Status == AlertStatus.Active)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByTripIdAndTypeAsync(int tripId, AlertType type, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Alert>()
            .AnyAsync(a => a.TripId == tripId && a.Type == type, cancellationToken);
    }
}