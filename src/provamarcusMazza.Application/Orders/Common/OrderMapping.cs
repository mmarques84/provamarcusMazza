using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Application.Orders.Common;

internal static class OrderMapping
{
    public static OrderResponse ToResponse(this Order order)
        => new(
            order.Id,
            order.CustomerId,
            order.Customer.Name,
            order.Status,
            order.CreatedAt,
            order.TotalAmount,
            order.Items.Select(i => new OrderItemResponse(
                i.Id, i.ProductName, i.Quantity, i.UnitPrice, i.Total)).ToList());
}
