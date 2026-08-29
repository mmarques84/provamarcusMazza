using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Users.Commands.CreateUser;
using provamarcusMazza.Application.Users.Common;
using provamarcusMazza.Application.Users.Queries.GetUsers;

namespace provamarcusMazza.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/users")]
public sealed class UsersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UserResponse>> Create(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<UserResponse>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => Ok(await sender.Send(new GetUsersQuery(page, pageSize), cancellationToken));
}
