using backSendify.Users.Application.Auth.Dtos;

namespace backSendify.Users.Application.Abstractions;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<bool> LogoutAsync(Guid userId, CancellationToken cancellationToken);
}
