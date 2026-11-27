using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using ChildrenPath.Platform.Tracking.Domain.Model.Aggregate;
using ChildrenPath.Platform.Tracking.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Tracking.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class TrackingSessionRepository(AppDbContext context) 
    : BaseRepository<TrackingSession>(context), ITrackingSessionRepository
{
    public async Task<IEnumerable<TrackingSession>> FindAllActiveAsync(CancellationToken cancellationToken = default)
    {
        return await Context.Set<TrackingSession>()
            .Include(t => t.Locations)
            .Include(t => t.ETAs)
            .Where(t => t.Status == TrackingStatus.Active)
            .ToListAsync(cancellationToken);
    }
}