using MediatR;
using provamarcusMazza.Application.Orders.Common;

namespace provamarcusMazza.Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderItem(
    string ProductName,
    int Quantity,
    decimal UnitPrice
    );

public sealed record CreateOrderCommand(
    Guid CustomerId,
    IReadOnlyList<CreateOrderItem> Items)
    : IRequest<OrderResponse>;
