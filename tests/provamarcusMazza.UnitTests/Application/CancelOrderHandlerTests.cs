using Moq;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Application.Orders.Commands.CancelOrder;
using provamarcusMazza.Domain.Entities;
using provamarcusMazza.Domain.Enums;

namespace provamarcusMazza.UnitTests.Application;

public sealed class CancelOrderHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCancelPendingOrder()
    {
        var order = Order.Create(
            Guid.NewGuid(),
            new[] { ("Produto", 1, 10m) });

        var orders = new Mock<IOrderRepository>();
        var uow = new Mock<IUnitOfWork>();

        orders
            .Setup(x => x.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var handler = new CancelOrderHandler(orders.Object, uow.Object);

        var result = await handler.Handle(
            new CancelOrderCommand(order.Id),
            CancellationToken.None);

        Assert.Equal(OrderStatus.Cancelled, result.Status);
        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
