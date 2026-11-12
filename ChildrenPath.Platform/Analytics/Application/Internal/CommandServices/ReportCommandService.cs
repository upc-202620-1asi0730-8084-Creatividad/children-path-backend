using ChildrenPath.Platform.Analytics.Application.CommandServices;
using ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;
using ChildrenPath.Platform.Analytics.Domain.Model.Commands;
using ChildrenPath.Platform.Analytics.Domain.Model.Entities;
using ChildrenPath.Platform.Analytics.Domain.Model.Errors;
using ChildrenPath.Platform.Analytics.Domain.Repositories;
using ChildrenPath.Platform.Shared.Domain;
using ChildrenPath.Platform.Shared.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenPath.Platform.Analytics.Application.Internal.CommandServices;

public class ReportCommandService(
    IReportRepository reportRepository,
    IUnitOfWork unitOfWork) 
    : IReportCommandService
{
    public async Task<Result<Report>> Handle(CreateReportCommand command, CancellationToken cancellationToken)
    {
        var report = new Report(command);
        
        // Simulamos la generación de métricas y KPIs base
        report.AddMetric(new Metric("Generated Records", 100, "records"));
        report.AddKpi(new KPI("QUALITY", "Service Quality", 90, 95));
        report.Generate();

        try
        {
            await reportRepository.AddAsync(report, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Report>.Success(report);
        }
        catch (OperationCanceledException)
        {
            return AnalyticsErrors.OperationCancelled.ToFailure<Report>();
        }
        catch (DbUpdateException)
        {
            return AnalyticsErrors.DatabaseError.ToFailure<Report>();
        }
        catch (Exception ex)
        {
            // Retornamos el error real temporalmente para debug
            return Result<Report>.Failure($"Error Real: {ex.Message} | Detalle: {ex.InnerException?.Message}");
        }
    }

    public async Task<Result<string>> Handle(ExportReportCommand command, CancellationToken cancellationToken)
    {
        var report = await reportRepository.FindByIdAsync(command.ReportId, cancellationToken);
        
        if (report is null)
            return AnalyticsErrors.ReportNotFound.ToFailure<string>();

        try
        {
            // Llamamos al método Export del Dominio
            var exportResult = report.Export(command.Format);
            return Result<string>.Success(exportResult);
        }
        catch (Exception)
        {
            return AnalyticsErrors.InternalServerError.ToFailure<string>();
        }
    }
}