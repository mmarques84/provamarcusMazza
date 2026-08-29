using MediatR;
using provamarcusMazza.Application.Common.Exceptions;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Application.Orders.Common;

namespace provamarcusMazza.Application.Orders.Commands.CancelOrder;

public sealed class CancelOrderHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CancelOrderCommand, OrderResponse>
{
    public async Task<OrderResponse> Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Order not found.");

        order.Cancel();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return order.ToResponse();
    }
}
