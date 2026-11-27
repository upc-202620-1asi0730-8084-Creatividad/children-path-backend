using ChildrenPath.Platform.Dashboard.Domain.Repositories;
using ChildrenPath.Platform.Shared.Infrastructure;
using ChildrenPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace ChildrenPath.Platform.Dashboard.Infrastructure.Persistence.EntityFrameworkCore.Repositories;


public class DashboardRepository(AppDbContext context) : IDashboardRepository
{
    protected readonly AppDbContext Context = context;
}