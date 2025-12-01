using backSendify.Users.Application.Users.Dtos;
using backSendify.Users.Application.Users.Requests;

namespace backSendify.Users.Application.Abstractions;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAsync(CancellationToken cancellationToken);
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken);
    Task<UserDto?> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
