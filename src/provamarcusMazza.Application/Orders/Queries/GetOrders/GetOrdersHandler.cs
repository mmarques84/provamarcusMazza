using MediatR;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Orders.Common;

namespace provamarcusMazza.Application.Orders.Queries.GetOrders;

public sealed class GetOrdersHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrdersQuery, PagedResult<OrderResponse>>
{
    public async Task<PagedResult<OrderResponse>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var (orders, totalCount) = await orderRepository.GetPagedAsync(
            request.Page,
            request.PageSize,
            request.CustomerName,
            request.CustomerId,
            request.Status,
            request.MinTotal,
            request.MaxTotal,
            cancellationToken);

        return new PagedResult<OrderResponse>(
            orders.Select(o => o.ToResponse()).ToList(),
            request.Page,
            request.PageSize,
            totalCount);
    }
}