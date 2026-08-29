using MediatR;
using provamarcusMazza.Application.Orders.Common;

namespace provamarcusMazza.Application.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid Id) : IRequest<OrderResponse>;
