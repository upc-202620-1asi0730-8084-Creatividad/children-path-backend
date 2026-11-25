using ChildrenPath.Platform.Shared.Domain.Model.Aggregates;
using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

public sealed class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData e, InterceptionResult<int> r)
    { Dispatch(e.Context).GetAwaiter().GetResult(); return base.SavingChanges(e, r); }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData e, InterceptionResult<int> r, CancellationToken ct = default)
    { await Dispatch(e.Context, ct); return await base.SavingChangesAsync(e, r, ct); }

    private async Task Dispatch(DbContext? ctx, CancellationToken ct = default)
    {
        if (ctx is null) return;
        var roots = ctx.ChangeTracker.Entries<IAggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any()).Select(e => e.Entity).ToList();
        var events = roots.SelectMany(r => r.DomainEvents).ToList();
        roots.ForEach(r => r.ClearDomainEvents());
        foreach (var ev in events) await mediator.PublishAsync(ev, ct);
    }
}