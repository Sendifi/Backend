using BackendSendify.Users.Domain.Model.Aggregates;
using BackendSendify.Users.Interfaces.REST.Resources;

namespace BackendSendify.Users.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResource(User entity) => new()
    {
        Id = entity.Id,
        Username = entity.Username,
        Email = entity.Email,
        Name = entity.Name,
        Role = entity.Role,
        Avatar = entity.Avatar,
        IsActive = entity.IsActive,
        CreatedAt = entity.CreatedAt,
        UpdatedAt = entity.UpdatedAt
    };
}

