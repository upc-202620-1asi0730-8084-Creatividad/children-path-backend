using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using ChildrenPath.Platform.Trips.Application.CommandServices;
using ChildrenPath.Platform.Trips.Application.QueryServices;
using ChildrenPath.Platform.Trips.Domain.Model.Commands;
using ChildrenPath.Platform.Trips.Domain.Model.Errors;
using ChildrenPath.Platform.Trips.Domain.Model.Queries;
using ChildrenPath.Platform.Trips.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Trips.Interfaces.Resources;
using ChildrenPath.Platform.Trips.Interfaces.Transform;
using ChildrenPath.Platform.Shared.Domain;
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
    [EndpointSummary("get all trips")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TripResource>))]
    public async Task<IActionResult> GetAllTrips(CancellationToken cancellationToken)
    {
        var trips = await tripQueryService.Handle(new GetAllTripsQuery(), cancellationToken);
        return Ok(trips.Select(TripResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("get a trip by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTripById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var trip = await tripQueryService.Handle(new GetTripByIdQuery(id), cancellationToken);
        if (trip is null) return NotFound();

        return Ok(TripResourceFromEntityAssembler.ToResourceFromEntity(trip));
    }

    [HttpGet("status/{status}")]
    [EndpointSummary("get trips by status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TripResource>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTripsByStatus([FromRoute] string status, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<TripStatus>(status, true, out var tripStatus))
        {
            return ResultActionResultAssembler.ToActionResult(
                this,
                Result<object>.Failure(TripErrors.InvalidStatus),
                _ => Ok());
        }

        var trips = await tripQueryService.Handle(new GetTripsByStatusQuery(tripStatus), cancellationToken);
        return Ok(trips.Select(TripResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpGet("shift/{shift}")]
    [EndpointSummary("get trips by shift")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TripResource>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTripsByShift([FromRoute] string shift, CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<TripShift>(shift, true, out var tripShift))
        {
            return ResultActionResultAssembler.ToActionResult(
                this,
                Result<object>.Failure(TripErrors.InvalidShift),
                _ => Ok());
        }

        var trips = await tripQueryService.Handle(new GetTripsByShiftQuery(tripShift), cancellationToken);
        return Ok(trips.Select(TripResourceFromEntityAssembler.ToResourceFromEntity));
    }

    [HttpPost]
    [EndpointSummary("create a new trip")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripResource resource, CancellationToken cancellationToken)
    {
        var command = CreateTripCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await tripCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            createdTrip => CreatedAtAction(
                nameof(GetTripById),
                new { id = createdTrip.Id },
                TripResourceFromEntityAssembler.ToResourceFromEntity(createdTrip)));
    }

    [HttpPatch("{id:int}/start")]
    [EndpointSummary("start a scheduled trip")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> StartTrip([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await tripCommandService.Handle(new StartTripCommand(id), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updatedTrip => Ok(TripResourceFromEntityAssembler.ToResourceFromEntity(updatedTrip)));
    }

    [HttpPatch("{id:int}/complete")]
    [EndpointSummary("complete an active trip")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CompleteTrip([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await tripCommandService.Handle(new CompleteTripCommand(id), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            completedTrip => Ok(TripResourceFromEntityAssembler.ToResourceFromEntity(completedTrip)));
    }

    [HttpPatch("{id:int}/cancel")]
    [EndpointSummary("cancel a trip")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelTrip([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await tripCommandService.Handle(new CancelTripCommand(id), cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            cancelledTrip => Ok(TripResourceFromEntityAssembler.ToResourceFromEntity(cancelledTrip)));
    }

    [HttpPost("{id:int}/stops")]
    [EndpointSummary("add a route stop to a trip")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TripResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddRouteStop(
        [FromRoute] int id,
        [FromBody] AddRouteStopResource resource,
        CancellationToken cancellationToken)
    {
        var command = new AddRouteStopCommand(id, resource.Name, resource.Address, resource.Order, resource.ScheduledTime);
        var result = await tripCommandService.Handle(command, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(
            this,
            result,
            updatedTrip => Ok(TripResourceFromEntityAssembler.ToResourceFromEntity(updatedTrip)));
    }
}