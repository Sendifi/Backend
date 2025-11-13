using backSendify.Users.Domain.Users.Enums;

namespace backSendify.Users.Application.Users.Requests;

public class UpdateUserRequest
{
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public UserRole? Role { get; set; }
    public string? Avatar { get; set; }
    public bool? IsActive { get; set; }
}
