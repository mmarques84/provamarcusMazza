using MediatR;
using Microsoft.AspNetCore.Mvc;
using provamarcusMazza.Application.Auth.Commands.Login;

namespace provamarcusMazza.Api.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginResponse>> Login(
        LoginCommand command,
        CancellationToken cancellationToken)
        => Ok(await sender.Send(command, cancellationToken));
}
