using MediatR;
using provamarcusMazza.Application.Common.Exceptions;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Application.Orders.Common;
using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Application.Orders.Commands.CreateOrder;

public sealed class CreateOrderHandler(
    IOrderRepository orderRepository,
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateOrderCommand, OrderResponse>
{
    public async Task<OrderResponse> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        if (!await customerRepository.ExistsAsync(request.CustomerId, cancellationToken))
            throw new NotFoundException("Customer not found.");

        var order = Order.Create(
            request.CustomerId,
            request.Items.Select(i => (i.ProductName, i.Quantity, i.UnitPrice)));

        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return order.ToResponse();
    }
}
