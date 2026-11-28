using System.Net.Mime;
using ChildrenPath.Platform.Drivers.Application.QueryServices;
using ChildrenPath.Platform.Drivers.Domain.Model.Queries;
using ChildrenPath.Platform.Drivers.Interfaces.Resources;
using ChildrenPath.Platform.Drivers.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Drivers.Interfaces.Rest;

[ApiController]
[Route("api/v1/drivers/management")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Driver Management")]
public class DriverManagementController(IDriverQueryService queryService) : ControllerBase
{
    [HttpGet("summary")]
    [EndpointSummary("get driver summary metrics")]
    [ProducesResponseType(typeof(DriverSummaryResource), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken)
    {
        var summary = await queryService.Handle(new GetDriverSummaryQuery(), cancellationToken);
        return Ok(DriverDashboardResourceAssembler.ToSummaryResource(summary));
    }

    [HttpGet("reviews")]
    [EndpointSummary("get drivers requiring review")]
    [ProducesResponseType(typeof(IEnumerable<DriverReviewResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReviews(CancellationToken cancellationToken)
    {
        var reviews = await queryService.Handle(new GetDriverReviewsQuery(), cancellationToken);
        return Ok(reviews.Select(DriverDashboardResourceAssembler.ToReviewResource));
    }

    [HttpGet("shifts")]
    [EndpointSummary("get recent driver activity")]
    [ProducesResponseType(typeof(IEnumerable<DriverShiftResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetShifts([FromQuery] int limit = 20, CancellationToken cancellationToken = default)
    {
        var activities = await queryService.Handle(new GetDriverShiftsQuery(limit), cancellationToken);
        return Ok(activities.Select(DriverDashboardResourceAssembler.ToShiftResource));
    }
}