using provamarcusMazza.Domain.Common;
using provamarcusMazza.Domain.Enums;

namespace provamarcusMazza.Domain.Entities;

public sealed class Order
{
    private readonly List<OrderItem> _items = [];

    private Order() { }

    private Order(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new DomainException("Customer is required.");

        Id = Guid.NewGuid();
        CustomerId = customerId;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    // Regra exigida pelo teste: o total é calculado no domínio.
    public decimal TotalAmount => _items.Sum(item => item.Total);

    public static Order Create(Guid customerId, IEnumerable<(string ProductName, int Quantity, decimal UnitPrice)> items)
    {
        var order = new Order(customerId);

        foreach (var item in items)
            order.AddItem(item.ProductName, item.Quantity, item.UnitPrice);

        if (order._items.Count == 0)
            throw new DomainException("Order must have at least one item.");

        return order;
    }

    public void AddItem(string productName, int quantity, decimal unitPrice)
        => _items.Add(new OrderItem(Id, productName, quantity, unitPrice));

    public void Cancel()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Only pending orders can be cancelled.");

        Status = OrderStatus.Cancelled;
    }
}
