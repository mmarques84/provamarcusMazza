using Microsoft.EntityFrameworkCore;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Domain.Entities;
using provamarcusMazza.Domain.Enums;

namespace provamarcusMazza.Infrastructure.Persistence.Repositories;

public sealed class OrderRepository(AppDbContext dbContext) : IOrderRepository
{
    public Task AddAsync(Order order, CancellationToken cancellationToken)
        => dbContext.Orders.AddAsync(order, cancellationToken).AsTask();

    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<(IReadOnlyList<Order> Items, int TotalCount)> GetPagedAsync(
      int page,
      int pageSize,
      string? customerName,
      Guid? customerId,
      OrderStatus? status,
      decimal? minTotal,
      decimal? maxTotal,
      CancellationToken cancellationToken)
    {
        var query = dbContext.Orders
            .AsNoTracking()
            .Include(x => x.Items)
            .Include(x => x.Customer)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(customerName))
        {
            query = query.Where(x =>
                x.Customer.Name.Contains(customerName));
        }

        if (customerId.HasValue)
        {
            query = query.Where(x =>
                x.CustomerId == customerId.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(x =>
                x.Status == status.Value);
        }

        if (minTotal.HasValue)
        {
            query = query.Where(x =>
                x.Items.Sum(i => i.Quantity * i.UnitPrice)
                >= minTotal.Value);
        }

        if (maxTotal.HasValue)
        {
            query = query.Where(x =>
                x.Items.Sum(i => i.Quantity * i.UnitPrice)
                <= maxTotal.Value);
        }

        query = query.OrderByDescending(x => x.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
