using Microsoft.EntityFrameworkCore;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Infrastructure.Persistence.Repositories;

public sealed class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        => dbContext.Users.FirstOrDefaultAsync(
            x => x.Email == email.Trim().ToLowerInvariant(),
            cancellationToken);

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
        => dbContext.Users.AnyAsync(
            x => x.Email == email.Trim().ToLowerInvariant(),
            cancellationToken);

    public Task AddAsync(User user, CancellationToken cancellationToken)
        => dbContext.Users.AddAsync(user, cancellationToken).AsTask();

    public async Task<(IReadOnlyList<User> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Users
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
