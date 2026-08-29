using MediatR;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Orders.Common;

namespace provamarcusMazza.Application.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResult<OrderResponse>>;
