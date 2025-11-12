using BackendSendify.Users.Domain.Model.Aggregates;
using BackendSendify.Users.Domain.Repositories;

namespace BackendSendify.Users.Application.Internal.InMemory;

public class InMemoryUserRepository : IUserRepository
{
    public Task<IEnumerable<User>> ListAsync(CancellationToken ct = default)
        => Task.FromResult<IEnumerable<User>>(InMemoryDataStore.Users.Values.OrderBy(u => u.CreatedAt));

    public Task<User?> FindByIdAsync(Guid id, CancellationToken ct = default)
    {
        InMemoryDataStore.Users.TryGetValue(id, out var user);
        return Task.FromResult<User?>(user);
    }

    public Task AddAsync(User entity, CancellationToken ct = default)
    {
        InMemoryDataStore.Users[entity.Id] = entity;
        return Task.CompletedTask;
    }

    public void Update(User entity)
    {
        InMemoryDataStore.Users[entity.Id] = entity;
    }

    public void Remove(User entity)
    {
        InMemoryDataStore.Users.TryRemove(entity.Id, out _);
    }

    public Task<User?> FindByEmailAsync(string email, CancellationToken ct = default)
    {
        var user = InMemoryDataStore.Users.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult<User?>(user);
    }

    public Task<User?> FindByUsernameAsync(string username, CancellationToken ct = default)
    {
        var user = InMemoryDataStore.Users.Values.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult<User?>(user);
    }

    public Task<bool> AnyByEmailAsync(string email, CancellationToken ct = default)
        => Task.FromResult(InMemoryDataStore.Users.Values.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));

    public Task<bool> AnyByUsernameAsync(string username, CancellationToken ct = default)
        => Task.FromResult(InMemoryDataStore.Users.Values.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)));
}

