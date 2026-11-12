using ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;
using ChildrenPath.Platform.Analytics.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Analytics.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Analytics.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ReportRepository(AppDbContext context)
    : BaseRepository<Report>(context), IReportRepository
{
    private readonly AppDbContext _context = context;

    // Importante: Incluir las entidades compuestas (Metrics y Kpis) al buscar
    public override async Task<Report?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Report>()
            .Include(r => r.Metrics)
            .Include(r => r.Kpis)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public override async Task<IEnumerable<Report>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Report>()
            .Include(r => r.Metrics)
            .Include(r => r.Kpis)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Report>> FindByTypeAsync(ReportType type, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Report>()
            .Include(r => r.Metrics)
            .Include(r => r.Kpis)
            .Where(r => r.Type == type)
            .ToListAsync(cancellationToken);
    }
}