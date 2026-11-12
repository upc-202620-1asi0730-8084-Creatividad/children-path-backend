using ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;
using ChildrenPath.Platform.Analytics.Domain.Model.Queries;

namespace ChildrenPath.Platform.Analytics.Application.QueryServices;

public interface IReportQueryService
{
    Task<Report?> Handle(GetReportByIdQuery query, CancellationToken cancellationToken);
    Task<IEnumerable<Report>> Handle(GetAllReportsQuery query, CancellationToken cancellationToken);
    
    // Query especial para el dashboard
    Task<IEnumerable<Report>> Handle(GetAnalyticsDashboardQuery query, CancellationToken cancellationToken);
}