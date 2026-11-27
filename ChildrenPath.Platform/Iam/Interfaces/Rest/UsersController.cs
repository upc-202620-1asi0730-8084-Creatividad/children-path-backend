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

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var user = await userQueryService.Handle(query, cancellationToken);

        if (user is null)
        {
            var errorResult = Shared.Domain.Result<Domain.Model.Aggregates.User>.Failure(
                Domain.Model.Errors.IamErrors.UserNotFound);
            return ResultActionResultAssembler.ToActionResult(this, errorResult, _ => BadRequest());
        }

        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }
}