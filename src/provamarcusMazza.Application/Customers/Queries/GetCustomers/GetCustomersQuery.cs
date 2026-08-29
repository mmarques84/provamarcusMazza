using MediatR;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Customers.Common;

namespace provamarcusMazza.Application.Customers.Queries.GetCustomers;

public sealed record GetCustomersQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResult<CustomerResponse>>;
