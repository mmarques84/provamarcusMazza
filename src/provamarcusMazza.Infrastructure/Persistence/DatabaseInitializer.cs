using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitialiseDatabaseAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        await db.Database.MigrateAsync();

        // Usuário fixo exigido pelo enunciado do teste (login: dev@martech.com / Senha@123).
        if (!await db.Users.AnyAsync(u => u.Email == "dev@martech.com"))
        {
            db.Users.Add(new User(
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                "dev@martech.com",
                passwordService.Hash("Senha@123")));
        }

        await db.SaveChangesAsync();
    }
}
