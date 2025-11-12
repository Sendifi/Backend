using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using BackendSendify.Users.Domain.Commands;
using BackendSendify.Users.Domain.Services;
using BackendSendify.Users.Interfaces.REST.Resources;
using BackendSendify.Users.Interfaces.REST.Transform;

namespace BackendSendify.Users.Interfaces.REST;

[ApiController]
[Route("[controller]")]
public class UsersController(IUserQueryService queryService, IUserCommandService commandService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(Summary = "Lista usuarios")]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers(CancellationToken ct)
    {
        var users = await queryService.Handle(ct);
        var resources = users.Select(UserResourceFromEntityAssembler.ToResource);
        return Ok(resources);
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Obtiene usuario por id")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var user = await queryService.Handle(id, ct);
        if (user is null) return NotFound();
        return Ok(UserResourceFromEntityAssembler.ToResource(user));
    }

    [Authorize]
    [HttpGet("me")]
    [SwaggerOperation(Summary = "Usuario actual (stub)")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Me(CancellationToken ct)
    {
        var userIdClaim = User.FindFirstValue("userId");
        if (string.IsNullOrWhiteSpace(userIdClaim)) return Unauthorized();
        if (!Guid.TryParse(userIdClaim, out var userId)) return Unauthorized();
        var user = await queryService.HandleMe(userId, ct);
        if (user is null) return Unauthorized();
        return Ok(UserResourceFromEntityAssembler.ToResource(user));
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Crea usuario")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateUserResource resource, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var command = CreateUserCommandFromResourceAssembler.ToCommand(resource);
        var result = await commandService.Handle(command, ct);
        if (!result.Success)
        {
            var error = result.Error ?? "Error";
            if (error.Contains("already in use", StringComparison.OrdinalIgnoreCase))
                return Conflict(new { message = error });
            return BadRequest(new { message = error });
        }

        var created = result.User!;
        var response = UserResourceFromEntityAssembler.ToResource(created);
        return Created($"/users/{created.Id}", response);
    }

    [HttpPatch("{id:guid}")]
    [SwaggerOperation(Summary = "Actualiza usuario")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserResource resource, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        var command = UpdateUserCommandFromResourceAssembler.ToCommand(id, resource);
        var result = await commandService.Handle(command, ct);
        if (!result.Success)
        {
            var error = result.Error ?? "Error";
            if (string.Equals(error, "User not found", StringComparison.OrdinalIgnoreCase))
                return NotFound();
            if (error.Contains("already in use", StringComparison.OrdinalIgnoreCase))
                return Conflict(new { message = error });
            return BadRequest(new { message = error });
        }
        return Ok(UserResourceFromEntityAssembler.ToResource(result.User!));
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Elimina usuario")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await commandService.Handle(new DeleteUserCommand(id), ct);
        if (!result.Success) return NotFound();
        return NoContent();
    }
}

