using MediatR;
using provamarcusMazza.Application.Common.Exceptions;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Application.Orders.Common;

namespace provamarcusMazza.Application.Orders.Queries.GetOrderById;

public sealed class GetOrderByIdHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrderByIdQuery, OrderResponse>
{
    public async Task<OrderResponse> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Order not found.");

        return order.ToResponse();
    }
}
