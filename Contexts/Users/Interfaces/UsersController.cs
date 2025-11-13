using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using backSendify.Users.Application.Abstractions;
using backSendify.Users.Application.Users.Dtos;
using backSendify.Users.Application.Users.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backSendify.Users.Interfaces;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers(CancellationToken cancellationToken)
    {
        var users = await _userService.GetAsync(cancellationToken);
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(id, cancellationToken);
        return user is null
            ? NotFound(new { message = "User not found" })
            : Ok(user);
    }

    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var parsedId))
        {
            return Unauthorized();
        }

        var user = await _userService.GetByIdAsync(parsedId, cancellationToken);
        return user is null
            ? NotFound(new { message = "User not found" })
            : Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<UserDto>> UpdateUser(Guid id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userService.UpdateAsync(id, request, cancellationToken);
            return user is null
                ? NotFound(new { message = "User not found" })
                : Ok(user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _userService.DeleteAsync(id, cancellationToken);
        return deleted
            ? Ok(new { success = true })
            : NotFound(new { message = "User not found" });
    }
}
