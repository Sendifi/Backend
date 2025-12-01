using backSendify.Users.Domain.Users.Entities;
using backSendify.Users.Domain.Users.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace backSendify.Shared.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<BackSendifyDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        if (!await context.Users.AnyAsync(cancellationToken))
        {
            var admin = new ApplicationUser
            {
                Email = "admin@sendify.com",
                Name = "Administrador",
                Username = "admin",
                Role = UserRole.Admin,
                IsActive = true,
                Avatar = "https://avatars.githubusercontent.com/u/1?v=4",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
