using Microsoft.AspNetCore.Mvc;
using ChildrenPath.Platform.Analytics.Application.CommandServices;
using ChildrenPath.Platform.Analytics.Application.QueryServices;
using ChildrenPath.Platform.Analytics.Domain.Model.Commands;
using ChildrenPath.Platform.Analytics.Domain.Model.Queries;
using ChildrenPath.Platform.Analytics.Interfaces.Resources;
using ChildrenPath.Platform.Analytics.Interfaces.Transform;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;

namespace ChildrenPath.Platform.Analytics.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class AnalyticsController(
    IReportCommandService reportCommandService,
    IReportQueryService reportQueryService,
    IAnalyticsDashboardQueryService analyticsDashboardQueryService)
    : ControllerBase
{
    [HttpGet("dashboard")]
    [EndpointSummary("get analytics dashboard")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnalyticsDashboardResource))]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await analyticsDashboardQueryService
            .Handle(new GetAnalyticsDashboardQuery(), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            snapshot => Ok(AnalyticsDashboardResourceFromSnapshotAssembler.ToResourceFromSnapshot(snapshot)));
    }

    [HttpGet("reports")]
    [EndpointSummary("list all reports")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ReportResource>))]
    public async Task<IActionResult> GetAllReports(CancellationToken cancellationToken)
    {
        var reports = await reportQueryService.Handle(new GetAllReportsQuery(), cancellationToken);
        return Ok(reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("reports/{id:int}")]
    [EndpointSummary("get a report by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReportResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var report = await reportQueryService.Handle(new GetReportByIdQuery(id), cancellationToken);
        if (report is null) return NotFound();
        return Ok(ReportResourceFromEntityAssembler.ToResourceFromEntity(report));
    }

    [HttpPost("reports")]
    [EndpointSummary("create a new report")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ReportResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReport(
        [FromBody] CreateReportResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await reportCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(this, result, report =>
        {
            var resource = ReportResourceFromEntityAssembler.ToResourceFromEntity(report);
            return CreatedAtAction(nameof(GetReportById), new { id = resource.Id }, resource);
        });
    }

    [HttpGet("reports/{id:int}/export")]
    [EndpointSummary("export a report to the given format")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExportReportResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExportReport(
        [FromRoute] int id,
        [FromQuery] string format = "pdf",
        CancellationToken cancellationToken = default)
    {
        var command = new ExportReportCommand(id, format);
        var result = await reportCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            message => Ok(new ExportReportResource(id, format, message)));
    }
}