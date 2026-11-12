using ChildrenPath.Platform.Trips.Application.CommandServices;
using ChildrenPath.Platform.Trips.Application.QueryServices;
using ChildrenPath.Platform.Trips.Domain.Model.Commands;
using ChildrenPath.Platform.Trips.Domain.Model.Queries;
using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Trips.Interfaces.Resources;
using ChildrenPath.Platform.Trips.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Trips.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
public class TripsController(
    ITripQueryService tripQueryService,
    ITripCommandService tripCommandService)
    : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all trips")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TripResource>))]
    public async Task<IActionResult> GetAllTrips(CancellationToken cancellationToken)
    {
        var query = new GetAllTripsQuery();
        var trips = await tripQueryService.Handle(query, cancellationToken);

        var resources = trips.Select(TripResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get a trip by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTripById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetTripByIdQuery(id);
        var trip = await tripQueryService.Handle(query, cancellationToken);

        if (trip is null) return NotFound();

        var resource = TripResourceFromEntityAssembler.ToResourceFromEntity(trip);
        return Ok(resource);
    }

    [HttpGet("status/{status}")]
    [EndpointSummary("Get trips by status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TripResource>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTripsByStatus([FromRoute] string status, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<TripStatus>(status, true, out var tripStatus))
            return BadRequest("Invalid trip status.");

        var query = new GetTripsByStatusQuery(tripStatus);
        var trips = await tripQueryService.Handle(query, cancellationToken);

        var resources = trips.Select(TripResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost]
    [EndpointSummary("Create a new trip")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripResource resource, CancellationToken cancellationToken)
    {
        var command = CreateTripCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await tripCommandService.Handle(command, cancellationToken);

        return TripActionResultAssembler.ToActionResult(
            this,
            result,
            createdTrip => CreatedAtAction(
                nameof(GetTripById),
                new { id = createdTrip.Id },
                TripResourceFromEntityAssembler.ToResourceFromEntity(createdTrip))
        );
    }

    [HttpPatch("{id:int}/start")]
    [EndpointSummary("Start a scheduled trip")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartTrip([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new StartTripCommand(id);
        var result = await tripCommandService.Handle(command, cancellationToken);

        return TripActionResultAssembler.ToActionResult(
            this,
            result,
            updatedTrip => Ok(TripResourceFromEntityAssembler.ToResourceFromEntity(updatedTrip))
        );
    }

    [HttpPatch("{id:int}/complete")]
    [EndpointSummary("Complete an active trip")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteTrip([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new CompleteTripCommand(id);
        var result = await tripCommandService.Handle(command, cancellationToken);

        return TripActionResultAssembler.ToActionResult(
            this,
            result,
            completedTrip => Ok(TripResourceFromEntityAssembler.ToResourceFromEntity(completedTrip))
        );
    }

    [HttpPatch("{id:int}/cancel")]
    [EndpointSummary("Cancel a trip")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTrip([FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new CancelTripCommand(id);
        var result = await tripCommandService.Handle(command, cancellationToken);

        return TripActionResultAssembler.ToActionResult(
            this,
            result,
            cancelledTrip => Ok(TripResourceFromEntityAssembler.ToResourceFromEntity(cancelledTrip))
        );
    }

    [HttpPost("{id:int}/stops")]
    [EndpointSummary("Add a route stop to a trip")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddRouteStop([FromRoute] int id, [FromBody] AddRouteStopResource resource, CancellationToken cancellationToken)
    {
        var command = new AddRouteStopCommand(id, resource.Name, resource.Address, resource.Order, resource.ScheduledTime);
        var result = await tripCommandService.Handle(command, cancellationToken);

        return TripActionResultAssembler.ToActionResult(
            this,
            result,
            updatedTrip => Ok(TripResourceFromEntityAssembler.ToResourceFromEntity(updatedTrip))
        );
    }
}