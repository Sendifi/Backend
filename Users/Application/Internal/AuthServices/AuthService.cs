using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BackendSendify.Users.Application.Internal.InMemory;
using BackendSendify.Users.Domain.Repositories;
using BackendSendify.Users.Domain.Services;

namespace BackendSendify.Users.Application.Internal.AuthServices;

public class AuthService(IUserRepository userRepository, string jwtKey, string issuer, string audience, int expiresMinutes) : IAuthService
{
    private readonly SymmetricSecurityKey _signingKey = new(Encoding.UTF8.GetBytes(jwtKey));

    public async Task<(bool Success, string? Error, Domain.Model.Aggregates.User? User, string? Token)> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var user = await userRepository.FindByEmailAsync(email, ct);
        if (user is null) return (false, "Invalid credentials", null, null);

        var incomingHash = PasswordHashing.Hash(password);
        if (!string.Equals(incomingHash, user.PasswordHash, StringComparison.Ordinal))
            return (false, "Invalid credentials", null, null);

        var token = GenerateToken(user);
        return (true, null, user, token);
    }

    public Task LogoutAsync(string token, CancellationToken ct = default)
    {
        // Stub: no server-side token store/blacklist
        return Task.CompletedTask;
    }

    private string GenerateToken(Domain.Model.Aggregates.User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new("userId", user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role)
        };

        var creds = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

