using provamarcusMazza.Domain.Enums;

namespace provamarcusMazza.Application.Orders.Common;

public sealed record OrderItemResponse(
    Guid Id,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Total);

public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    OrderStatus Status,
    DateTime CreatedAt,
    decimal TotalAmount,
    IReadOnlyList<OrderItemResponse> Items);
