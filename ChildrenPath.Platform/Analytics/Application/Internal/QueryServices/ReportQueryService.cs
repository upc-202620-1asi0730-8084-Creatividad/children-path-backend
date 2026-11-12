using ChildrenPath.Platform.Analytics.Application.QueryServices;
using ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;
using ChildrenPath.Platform.Analytics.Domain.Model.Queries;
using ChildrenPath.Platform.Analytics.Domain.Repositories;

namespace ChildrenPath.Platform.Analytics.Application.Internal.QueryServices;

public class ReportQueryService(IReportRepository reportRepository) : IReportQueryService
{
    public async Task<Report?> Handle(GetReportByIdQuery query, CancellationToken cancellationToken)
    {
        return await reportRepository.FindByIdAsync(query.ReportId, cancellationToken);
    }

    public async Task<IEnumerable<Report>> Handle(GetAllReportsQuery query, CancellationToken cancellationToken)
    {
        return await reportRepository.ListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Report>> Handle(GetAnalyticsDashboardQuery query, CancellationToken cancellationToken)
    {
        return await reportRepository.ListAsync(cancellationToken);
    }
}