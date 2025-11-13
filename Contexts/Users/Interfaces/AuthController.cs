using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using backSendify.Users.Application.Abstractions;
using backSendify.Users.Application.Auth.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backSendify.Users.Interfaces;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginAsync(request, cancellationToken);
        return response is null
            ? Unauthorized(new { message = "Invalid credentials" })
            : Ok(response);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var parsed))
        {
            return Unauthorized();
        }

        await _authService.LogoutAsync(parsed, cancellationToken);
        return Ok(new { success = true });
    }
}
