using provamarcusMazza.Domain.Common;
using provamarcusMazza.Domain.Entities;
using provamarcusMazza.Domain.Enums;

namespace provamarcusMazza.UnitTests.Domain;

public sealed class OrderTests
{
    [Fact]
    public void Create_ShouldCalculateTotalAmountInDomain()
    {
        var order = Order.Create(
            Guid.NewGuid(),
            new[]
            {
                ("Produto A", 2, 10m),
                ("Produto B", 1, 5m)
            });

        Assert.Equal(25m, order.TotalAmount);
        Assert.Equal(OrderStatus.Pending, order.Status);
    }

    [Fact]
    public void Create_WithoutItems_ShouldThrow()
    {
        Assert.Throws<DomainException>(() =>
            Order.Create(
                Guid.NewGuid(),
                Array.Empty<(string ProductName, int Quantity, decimal UnitPrice)>()));
    }

    [Fact]
    public void Cancel_WhenPending_ShouldCancel()
    {
        var order = Order.Create(
            Guid.NewGuid(),
            new[] { ("Produto", 1, 10m) });

        order.Cancel();

        Assert.Equal(OrderStatus.Cancelled, order.Status);
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_ShouldThrow()
    {
        var order = Order.Create(
            Guid.NewGuid(),
            new[] { ("Produto", 1, 10m) });

        order.Cancel();

        Assert.Throws<DomainException>(() => order.Cancel());
    }
}
