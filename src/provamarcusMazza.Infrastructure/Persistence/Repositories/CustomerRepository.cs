using Microsoft.EntityFrameworkCore;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Infrastructure.Persistence.Repositories;

public sealed class CustomerRepository(AppDbContext dbContext) : ICustomerRepository
{
    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken)
        => dbContext.Customers.AnyAsync(x => x.Id == id, cancellationToken);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        => dbContext.Customers.AnyAsync(
            x => x.Email == email.Trim().ToLowerInvariant(),
            cancellationToken);

    public Task AddAsync(Customer customer, CancellationToken cancellationToken)
        => dbContext.Customers.AddAsync(customer, cancellationToken).AsTask();

    public async Task<(IReadOnlyList<Customer> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Customers
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
