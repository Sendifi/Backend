using BackendSendify.Users.Domain.Model.Aggregates;
using BackendSendify.Users.Domain.Repositories;
using BackendSendify.Users.Domain.Services;

namespace BackendSendify.Users.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public Task<IEnumerable<User>> Handle(CancellationToken ct = default)
        => userRepository.ListAsync(ct);

    public Task<User?> Handle(Guid id, CancellationToken ct = default)
        => userRepository.FindByIdAsync(id, ct);

    public Task<User?> HandleMe(Guid userId, CancellationToken ct = default)
        => userRepository.FindByIdAsync(userId, ct);
}

