using System.ComponentModel.DataAnnotations;

namespace BackendSendify.Users.Interfaces.REST.Resources;

public class UserResource
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Name { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateUserResource
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(120)]
    public string? Name { get; set; }

    [MaxLength(512)]
    public string? Avatar { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "viewer";

    public bool IsActive { get; set; } = true;
}

public class UpdateUserResource
{
    [MaxLength(50)]
    public string? Username { get; set; }

    [EmailAddress]
    [MaxLength(255)]
    public string? Email { get; set; }

    [MaxLength(120)]
    public string? Name { get; set; }

    [MaxLength(512)]
    public string? Avatar { get; set; }

    public string? Role { get; set; }
    public bool? IsActive { get; set; }
}

public class LoginRequestResource
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseResource
{
    public UserResource User { get; set; } = default!;
    public string Token { get; set; } = string.Empty;
}

