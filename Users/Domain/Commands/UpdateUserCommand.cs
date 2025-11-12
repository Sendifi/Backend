namespace BackendSendify.Users.Domain.Commands;

public record UpdateUserCommand(
    Guid Id,
    string? Username,
    string? Email,
    string? Name,
    string? Role,
    string? Avatar,
    bool? IsActive
);

