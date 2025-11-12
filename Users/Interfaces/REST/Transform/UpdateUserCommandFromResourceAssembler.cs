using BackendSendify.Users.Domain.Commands;
using BackendSendify.Users.Interfaces.REST.Resources;

namespace BackendSendify.Users.Interfaces.REST.Transform;

public static class UpdateUserCommandFromResourceAssembler
{
    public static UpdateUserCommand ToCommand(Guid id, UpdateUserResource resource)
        => new(
            Id: id,
            Username: resource.Username,
            Email: resource.Email,
            Name: resource.Name,
            Role: resource.Role,
            Avatar: resource.Avatar,
            IsActive: resource.IsActive);
}

