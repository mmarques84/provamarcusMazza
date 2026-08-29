using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Application.Common.Interfaces;

public interface ICustomerRepository
{
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddAsync(Customer customer, CancellationToken cancellationToken);
    Task<(IReadOnlyList<Customer> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
}
