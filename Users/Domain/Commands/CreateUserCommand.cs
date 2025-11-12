namespace BackendSendify.Users.Domain.Commands;

public record CreateUserCommand(
    string Username,
    string Email,
    string? Name,
    string Role,
    string? Avatar,
    bool IsActive,
    string Password
);

