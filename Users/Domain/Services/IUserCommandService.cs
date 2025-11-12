using BackendSendify.Users.Domain.Commands;
using BackendSendify.Users.Domain.Model.Aggregates;

namespace BackendSendify.Users.Domain.Services;

public interface IUserCommandService
{
    Task<(bool Success, string? Error, User? User)> Handle(CreateUserCommand command, CancellationToken ct = default);
    Task<(bool Success, string? Error, User? User)> Handle(UpdateUserCommand command, CancellationToken ct = default);
    Task<(bool Success, string? Error)> Handle(DeleteUserCommand command, CancellationToken ct = default);
}

