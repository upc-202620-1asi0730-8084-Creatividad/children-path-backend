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
    IAlertCommandService alertCommandService)
    : ControllerBase
{
    [HttpGet("{id:int}")]
    [EndpointSummary("Get an alert by id")]
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
    [EndpointSummary("Get alerts by trip id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AlertResource>))]
    public async Task<IActionResult> GetAlertsByTripId([FromRoute] int tripId, CancellationToken cancellationToken)
    {
        var query = new GetAlertsByTripIdQuery(tripId);
        var alerts = await alertQueryService.Handle(query, cancellationToken);

        var resources = alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost]
    [EndpointSummary("Create a new alert")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AlertResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAlert([FromBody] CreateAlertResource resource,
        CancellationToken cancellationToken)
    {
        var command = CreateAlertCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await alertCommandService.Handle(command, cancellationToken);

        return AlertActionResultAssembler.ToActionResult(
            this,
            result,
            createdAlert => CreatedAtAction(
                nameof(GetAlertById),
                new { id = createdAlert.Id },
                AlertResourceFromEntityAssembler.ToResourceFromEntity(createdAlert))
        );
    }

    [HttpPatch("{id:int}/resolve")]
    [EndpointSummary("Resolve an alert")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlertResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResolveAlert([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new ResolveAlertCommand(id);
        var result = await alertCommandService.Handle(command, cancellationToken);

        return AlertActionResultAssembler.ToActionResult(
            this,
            result,
            resolvedAlert => Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(resolvedAlert))
        );
    }

    [HttpGet("/api/v1/alertNotificationCenter")]
    [EndpointSummary("Get Dashboard for Notifications Frontend")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlertDashboardResource))]
    public async Task<IActionResult> GetDashboard(CancellationToken cancellationToken)
    {
        var alerts = await alertQueryService.Handle(new GetAllActiveAlertsQuery(), cancellationToken);

        var resource = AlertDashboardAssembler.ToDashboardResource(alerts);

        return Ok(resource);
    }
    
    [HttpPatch("{id:int}/read")]
    [EndpointSummary("Mark an alert as read")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AlertResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAlertAsRead([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new MarkAlertAsReadCommand(id);
        var result = await alertCommandService.Handle(command, cancellationToken);

        return AlertActionResultAssembler.ToActionResult(
            this,
            result,
            updatedAlert => Ok(AlertResourceFromEntityAssembler.ToResourceFromEntity(updatedAlert))
        );
    }

}