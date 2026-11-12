using ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;
using ChildrenPath.Platform.Analytics.Domain.Model.Commands;
using ChildrenPath.Platform.Shared.Domain;

namespace ChildrenPath.Platform.Analytics.Application.CommandServices;

public interface IReportCommandService
{
    Task<Result<Report>> Handle(CreateReportCommand command, CancellationToken cancellationToken);
    Task<Result<string>> Handle(ExportReportCommand command, CancellationToken cancellationToken);
}