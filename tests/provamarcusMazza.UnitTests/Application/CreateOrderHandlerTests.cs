using Moq;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Application.Orders.Commands.CreateOrder;
using Xunit;

namespace provamarcusMazza.UnitTests.Application;

public sealed class CreateOrderHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateOrder()
    {
        var customerId = Guid.NewGuid();

        var orders = new Mock<IOrderRepository>();
        var customers = new Mock<ICustomerRepository>();
        var uow = new Mock<IUnitOfWork>();

        customers
            .Setup(x => x.ExistsAsync(customerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateOrderHandler(
            orders.Object,
            customers.Object,
            uow.Object);

        var result = await handler.Handle(
            new CreateOrderCommand(
                customerId,
                [new CreateOrderItem("Notebook", 2, 100m)]),
            CancellationToken.None);

        Assert.Equal(customerId, result.CustomerId);
        Assert.Equal(200m, result.TotalAmount);

        orders.Verify(
            x => x.AddAsync(It.IsAny<provamarcusMazza.Domain.Entities.Order>(), It.IsAny<CancellationToken>()),
            Times.Once);

        uow.Verify(
            x => x.SaveChangesAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
