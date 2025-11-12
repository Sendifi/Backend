using BackendSendify.Users.Domain.Model.Aggregates;

namespace BackendSendify.Users.Domain.Services;

public interface IUserQueryService
{
    Task<IEnumerable<User>> Handle(CancellationToken ct = default);
    Task<User?> Handle(Guid id, CancellationToken ct = default);
    Task<User?> HandleMe(Guid userId, CancellationToken ct = default);
}

