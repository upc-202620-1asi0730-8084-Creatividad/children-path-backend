using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using ChildrenPath.Platform.Alerts.Application.CommandServices;
using ChildrenPath.Platform.Alerts.Application.QueryServices;
using ChildrenPath.Platform.Alerts.Domain.Model.Commands;
using ChildrenPath.Platform.Alerts.Domain.Model.Queries;
using ChildrenPath.Platform.Alerts.Interfaces.Resources;
using ChildrenPath.Platform.Alerts.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Alerts.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class AlertsController(
    IAlertQueryService alertQueryService,
    IAlertCommandService alertCommandService,
    IAlertDashboardQueryService alertDashboardQueryService) 
    : ControllerBase
{
    [HttpGet("{id:int}")]
    [EndpointSummary("get an alert by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlertResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAlertById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetAlertByIdQuery(id);
        var alert = await alertQueryService.Handle(query, cancellationToken);

        if (alert is null) return NotFound();

        var resource = AlertResourceFromEntityAssembler.ToResourceFromEntity(alert);
        return Ok(resource);
    }

    [HttpGet("trip/{tripId:int}")]
    [EndpointSummary("get alerts by trip id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AlertResource>))]
    public async Task<IActionResult> GetAlertsByTripId([FromRoute] int tripId, CancellationToken cancellationToken)
    {
        var query = new GetAlertsByTripIdQuery(tripId);
        var alerts = await alertQueryService.Handle(query, cancellationToken);

        var resources = alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost]
    [EndpointSummary("create a new alert")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AlertResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAlert([FromBody] CreateAlertResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateAlertCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await alertCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            createdAlert => CreatedAtAction(
                nameof(GetAlertById),
                new { id = createdAlert.Id },
                AlertResourceFromEntityAssembler.ToResourceFromEntity(createdAlert))
        );
    }

    [HttpPatch("{id:int}/resolve")]
    [EndpointSummary("resolve an alert")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlertResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResolveAlert([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new ResolveAlertCommand(id);
        var result = await alertCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            resolvedAlert => Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(resolvedAlert))
        );
    }

    [HttpGet("notification-center")]              
    [EndpointSummary("get notification center dashboard")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlertDashboardResource))]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var result = await alertDashboardQueryService
            .Handle(new GetAlertDashboardQuery(), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            snapshot => Ok(AlertDashboardAssembler.ToDashboardResource(snapshot)));
    }
    
    [HttpPatch("{id:int}/read")]
    [EndpointSummary("mark an alert as read")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlertResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAlertAsRead([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new MarkAlertAsReadCommand(id);
        var result = await alertCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updatedAlert => Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(updatedAlert))
        );
    }

}