using MediatR;
using provamarcusMazza.Application.Orders.Common;

namespace provamarcusMazza.Application.Orders.Commands.CancelOrder;

public sealed record CancelOrderCommand(Guid Id) : IRequest<OrderResponse>;
