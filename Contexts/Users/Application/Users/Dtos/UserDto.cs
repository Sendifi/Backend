using backSendify.Users.Domain.Users.Enums;

namespace backSendify.Users.Application.Users.Dtos;

public record UserDto(
    Guid Id,
    string Email,
    string Name,
    string? Username,
    UserRole Role,
    string? Avatar,
    bool IsActive,
    DateTime CreatedAt,
    DateTime UpdatedAt);
