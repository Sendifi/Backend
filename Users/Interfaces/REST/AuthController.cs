using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using BackendSendify.Users.Domain.Services;
using BackendSendify.Users.Interfaces.REST.Resources;
using BackendSendify.Users.Interfaces.REST.Transform;

namespace BackendSendify.Users.Interfaces.REST;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Autenticar usuario (stub)")]
    [ProducesResponseType(typeof(LoginResponseResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestResource request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var (success, error, user, token) = await authService.LoginAsync(request.Email, request.Password, ct);
        if (!success || user is null || token is null)
            return Unauthorized(new { message = error ?? "Invalid credentials" });

        var resource = new LoginResponseResource
        {
            User = UserResourceFromEntityAssembler.ToResource(user),
            Token = token
        };
        return Ok(resource);
    }

    [HttpPost("logout")]
    [SwaggerOperation(Summary = "Cerrar sesión (stub)")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout([FromHeader(Name = "Authorization")] string? authorization, CancellationToken ct)
    {
        var token = authorization?.StartsWith("Bearer ") == true ? authorization![7..] : null;
        await authService.LogoutAsync(token ?? string.Empty, ct);
        return NoContent();
    }
}

