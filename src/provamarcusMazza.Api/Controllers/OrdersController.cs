using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Orders.Commands.CancelOrder;
using provamarcusMazza.Application.Orders.Commands.CreateOrder;
using provamarcusMazza.Application.Orders.Common;
using provamarcusMazza.Application.Orders.Queries.GetOrderById;
using provamarcusMazza.Application.Orders.Queries.GetOrders;
using provamarcusMazza.Domain.Enums;

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



    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
        => Ok(await sender.Send(new GetOrderByIdQuery(id), cancellationToken));

    [HttpGet]
    public async Task<ActionResult<PagedResult<OrderResponse>>> Get(
    [FromQuery] string? customerName,
    [FromQuery] Guid? custmoerid,
    [FromQuery] OrderStatus? status,
    [FromQuery] decimal? minTotal,
    [FromQuery] decimal? maxTotal,
    [FromQuery] DateTime? createdFrom,
    [FromQuery] DateTime? createdTo,
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        var query = new GetOrdersQuery(
            customerName, 
            custmoerid,
            status,
            minTotal,
            maxTotal,
            createdFrom,
            createdTo,
            page,
            pageSize);

        return Ok(await sender.Send(query, cancellationToken));
    }
    [HttpPatch("{id:guid}/cancel")]
    public async Task<ActionResult<OrderResponse>> Cancel(
        Guid id,
        CancellationToken cancellationToken)
        => Ok(await sender.Send(new CancelOrderCommand(id), cancellationToken));
}
