using BackendSendify.Users.Domain.Model.Aggregates;

namespace BackendSendify.Users.Domain.Services;

public interface IAuthService
{
    Task<(bool Success, string? Error, User? User, string? Token)> LoginAsync(string email, string password, CancellationToken ct = default);
    Task LogoutAsync(string token, CancellationToken ct = default);
}

