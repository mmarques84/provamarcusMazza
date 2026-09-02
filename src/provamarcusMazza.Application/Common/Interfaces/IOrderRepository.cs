using provamarcusMazza.Domain.Entities;
using provamarcusMazza.Domain.Enums;

namespace provamarcusMazza.Application.Common.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(Order order, CancellationToken cancellationToken);
    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Order> Items, int TotalCount)> GetPagedAsync(
       int page,
       int pageSize,
       string? customerName,
       Guid? customerId,
       OrderStatus? status,
       decimal? minTotal,
       decimal? maxTotal,
       CancellationToken cancellationToken);
}
