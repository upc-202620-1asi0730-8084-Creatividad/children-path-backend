using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using ChildrenPath.Platform.Trips.Domain.Model.Aggregate;
using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Trips.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Trips.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TripRepository(AppDbContext context)
    : BaseRepository<Trip>(context), ITripRepository
{
    private readonly AppDbContext _context = context;

    public override async Task<Trip?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Trip>()
            .Include(t => t.Stops)
            .Include(t => t.IncidentLogs)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Trip>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Trip>()
            .Include(t => t.Stops)
            .Include(t => t.IncidentLogs)
            .ToListAsync(cancellationToken);
    }

    public async Task<Trip?> FindByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Trip>()
            .Include(t => t.Stops)
            .Include(t => t.IncidentLogs)
            .FirstOrDefaultAsync(t => t.Code == code, cancellationToken);
    }

    public async Task<IEnumerable<Trip>> FindByStatusAsync(TripStatus status, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Trip>()
            .Include(t => t.Stops)
            .Include(t => t.IncidentLogs)
            .Where(t => t.Status == status)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Trip>> FindByShiftAsync(TripShift shift, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Trip>()
            .Include(t => t.Stops)
            .Include(t => t.IncidentLogs)
            .Where(t => t.Shift == shift)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Trip>()
            .AnyAsync(t => t.Code == code, cancellationToken);
    }
}