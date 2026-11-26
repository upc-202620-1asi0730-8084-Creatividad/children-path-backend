using ChildrenPath.Platform.Drivers.Domain.Model.Aggregates;
using ChildrenPath.Platform.Drivers.Domain.Model.Entities;
using ChildrenPath.Platform.Drivers.Domain.Model.Queries;
using ChildrenPath.Platform.Drivers.Domain.Model.ReadModels;

namespace ChildrenPath.Platform.Drivers.Application.QueryServices;

public interface IDriverQueryService
{
    Task<IEnumerable<Driver>> Handle(GetAllDriversQuery query, CancellationToken cancellationToken = default);
    Task<Driver?> Handle(GetDriverByIdQuery query, CancellationToken cancellationToken = default);
    Task<DriverSummary> Handle(GetDriverSummaryQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<DriverReview>> Handle(GetDriverReviewsQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<DriverActivity>> Handle(GetDriverShiftsQuery query, CancellationToken cancellationToken = default);
}