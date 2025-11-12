using Microsoft.EntityFrameworkCore;
using BackendSendify.Shared.Infrastructure.Persistence.EFC;
using BackendSendify.Users.Domain.Model.Aggregates;
using BackendSendify.Users.Domain.Repositories;

namespace BackendSendify.Users.Infrastructure.Persistence.EFC.Repositories;

public class EfCoreUserRepository(AppDbContext context) : IUserRepository
{
    public async Task<IEnumerable<User>> ListAsync(CancellationToken ct = default)
        => await context.Users.AsNoTracking().OrderBy(u => u.CreatedAt).ToListAsync(ct);

    public Task<User?> FindByIdAsync(Guid id, CancellationToken ct = default)
        => context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task AddAsync(User entity, CancellationToken ct = default)
    {
        await context.Users.AddAsync(entity, ct);
    }

    public void Update(User entity)
    {
        context.Users.Update(entity);
    }

    public void Remove(User entity)
    {
        context.Users.Remove(entity);
    }

    public Task<User?> FindByEmailAsync(string email, CancellationToken ct = default)
        => context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<User?> FindByUsernameAsync(string username, CancellationToken ct = default)
        => context.Users.FirstOrDefaultAsync(u => u.Username == username, ct);

    public Task<bool> AnyByEmailAsync(string email, CancellationToken ct = default)
        => context.Users.AnyAsync(u => u.Email == email, ct);

    public Task<bool> AnyByUsernameAsync(string username, CancellationToken ct = default)
        => context.Users.AnyAsync(u => u.Username == username, ct);
}

