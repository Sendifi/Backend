using BackendSendify.Users.Domain.Commands;
using BackendSendify.Users.Interfaces.REST.Resources;

namespace BackendSendify.Users.Interfaces.REST.Transform;

public static class CreateUserCommandFromResourceAssembler
{
    public static CreateUserCommand ToCommand(CreateUserResource resource)
        => new(
            Username: resource.Username,
            Email: resource.Email,
            Name: resource.Name,
            Role: resource.Role,
            Avatar: resource.Avatar,
            IsActive: resource.IsActive,
            Password: resource.Password);
}

