using backSendify.Shared.Domain.Common;
using backSendify.Users.Domain.Users.Enums;

namespace backSendify.Users.Domain.Users.Entities;

public class ApplicationUser : AuditableEntity
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Username { get; set; }
    public UserRole Role { get; set; } = UserRole.Viewer;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public bool IsActive { get; set; } = true;
}
