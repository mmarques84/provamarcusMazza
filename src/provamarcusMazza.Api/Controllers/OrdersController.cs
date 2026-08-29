using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Orders.Commands.CancelOrder;
using provamarcusMazza.Application.Orders.Commands.CreateOrder;
using provamarcusMazza.Application.Orders.Common;
using provamarcusMazza.Application.Orders.Queries.GetOrderById;
using provamarcusMazza.Application.Orders.Queries.GetOrders;

namespace provamarcusMazza.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public sealed class OrdersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(
        CreateOrderCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<OrderResponse>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => Ok(await sender.Send(new GetOrdersQuery(page, pageSize), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
        => Ok(await sender.Send(new GetOrderByIdQuery(id), cancellationToken));

    [HttpPatch("{id:guid}/cancel")]
    public async Task<ActionResult<OrderResponse>> Cancel(
        Guid id,
        CancellationToken cancellationToken)
        => Ok(await sender.Send(new CancelOrderCommand(id), cancellationToken));
}
