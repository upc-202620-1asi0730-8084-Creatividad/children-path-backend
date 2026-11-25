using System.Net.Mime;
using ChildrenPath.Platform.Iam.Application.CommandServices;
using ChildrenPath.Platform.Iam.Interfaces.Rest.Resources;
using ChildrenPath.Platform.Iam.Interfaces.Rest.Transform;
using ChildrenPath.Platform.Shared.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChildrenPath.Platform.Iam.Interfaces.Rest;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Authentication")]
public class TokensController(IUserCommandService userCommandService) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateToken([FromBody] SignInResource signInResource, CancellationToken cancellationToken)
    {
        var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
        var result = await userCommandService.Handle(signInCommand, cancellationToken);

        return ResultActionResultAssembler.ToActionResult(this, result, userAndToken =>
        {
            var authenticatedUser = AuthenticatedUserResourceFromEntityAssembler
                .ToResourceFromEntity(userAndToken.user, userAndToken.token);
            
            return Ok(authenticatedUser);
        });
    }
}