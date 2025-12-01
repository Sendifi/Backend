using System.ComponentModel.DataAnnotations;
using backSendify.Users.Domain.Users.Enums;

namespace backSendify.Users.Application.Users.Requests;

public class CreateUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Username { get; set; }

    [Required]
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.Operator;
    public string? Avatar { get; set; }
    public bool IsActive { get; set; } = true;
}
