using BackendSendify.Shared.Domain.Repositories;
using BackendSendify.Users.Domain.Model.Aggregates;

namespace BackendSendify.Users.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User, Guid>
{
    Task<User?> FindByEmailAsync(string email, CancellationToken ct = default);
    Task<User?> FindByUsernameAsync(string username, CancellationToken ct = default);
    Task<bool> AnyByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> AnyByUsernameAsync(string username, CancellationToken ct = default);
}

