using MediatR;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Orders.Common;
using provamarcusMazza.Domain.Enums;

namespace provamarcusMazza.Application.Orders.Queries.GetOrders;

public sealed record GetOrdersQuery(
    string? CustomerName = null,
    Guid? CustomerId = null,
    OrderStatus? Status = null,
    decimal? MinTotal = null,
    decimal? MaxTotal = null,
    DateTime? CreatedFrom = null,
    DateTime? CreatedTo = null,
    int Page = 1,
    int PageSize = 10
) : IRequest<PagedResult<OrderResponse>>;
