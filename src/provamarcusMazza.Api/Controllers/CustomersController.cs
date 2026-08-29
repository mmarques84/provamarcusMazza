using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Customers.Commands.CreateCustomer;
using provamarcusMazza.Application.Customers.Common;
using provamarcusMazza.Application.Customers.Queries.GetCustomers;

namespace provamarcusMazza.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/customers")]
public sealed class CustomersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(
        CreateCustomerCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<CustomerResponse>>> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
        => Ok(await sender.Send(new GetCustomersQuery(page, pageSize), cancellationToken));
}
