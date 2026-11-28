using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Dashboard.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static void ApplyDashboardConfiguration(this ModelBuilder builder)
    {
        // Dashboard no persiste datos propios.
        // Consume información agregada de otros Bounded Contexts (Trips, Fleet, Alerts).
        // No hay tablas que mapear para este contexto.
    }
}