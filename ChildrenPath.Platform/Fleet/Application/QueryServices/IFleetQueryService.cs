using ChildrenPath.Platform.Fleet.Domain.Model.Entities;
using ChildrenPath.Platform.Fleet.Domain.Model.Queries;

namespace ChildrenPath.Platform.Fleet.Application.QueryServices;

public interface IFleetQueryService
{
    Task<FleetSummary> Handle(GetFleetSummaryQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<MaintenanceAlert>> Handle(GetAllMaintenanceAlertsQuery query, CancellationToken cancellationToken = default);
    Task<FleetDashboardData> Handle(GetFleetDashboardQuery query, CancellationToken cancellationToken = default);
}
