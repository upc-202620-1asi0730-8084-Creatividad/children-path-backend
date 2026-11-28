using System.Net.Mime;
using ChildrenPath.Platform.Iam.Application.CommandServices;
using ChildrenPath.Platform.Iam.Application.QueryServices;
using ChildrenPath.Platform.Iam.Domain.Model.Queries;
using ChildrenPath.Platform.Iam.Interfaces.Rest.Resources;
using ChildrenPath.Platform.Iam.Interfaces.Rest.Transform;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("User Management")]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [EndpointSummary("register a new user")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResource))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateUser([FromBody] SignUpResource signUpResource, CancellationToken cancellationToken)
    {
        var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
        var result = await userCommandService.Handle(signUpCommand, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(this, result, user =>
        {
            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
            return CreatedAtAction(nameof(GetUserById), new { id = userResource.Id }, userResource);
        });
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointSummary("get a user by id")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResource))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await userQueryService.Handle(query, cancellationToken);

        if (user is null) return NotFound();

        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }
}