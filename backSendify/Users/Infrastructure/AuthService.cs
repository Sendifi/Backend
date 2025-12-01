using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using backSendify.Shared.Application.Mapping;
using backSendify.Shared.Infrastructure.Persistence;
using backSendify.Users.Application.Abstractions;
using backSendify.Users.Application.Auth.Dtos;
using backSendify.Users.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace backSendify.Users.Infrastructure;

public class AuthService : IAuthService
{
    private readonly BackSendifyDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthService(BackSendifyDbContext context, IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == normalizedEmail, cancellationToken);
        if (user == null || !user.IsActive)
        {
            return null;
        }

        var validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!validPassword)
        {
            return null;
        }

        var token = GenerateToken(user);
        return new LoginResponseDto(user.ToDto(), token);
    }

    public Task<bool> LogoutAsync(Guid userId, CancellationToken cancellationToken)
    {
        // Token invalidation handled on client side (stateless JWT)
        return Task.FromResult(true);
    }

    private string GenerateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString()),
            new("name", user.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
