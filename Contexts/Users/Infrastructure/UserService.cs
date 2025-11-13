using backSendify.Shared.Application.Mapping;
using backSendify.Shared.Infrastructure.Persistence;
using backSendify.Users.Application.Abstractions;
using backSendify.Users.Application.Users.Dtos;
using backSendify.Users.Application.Users.Requests;
using backSendify.Users.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace backSendify.Users.Infrastructure;

public class UserService : IUserService
{
    private readonly BackSendifyDbContext _context;

    public UserService(BackSendifyDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> GetAsync(CancellationToken cancellationToken)
    {
        var users = await _context.Users.AsNoTracking().OrderBy(u => u.Email).ToListAsync(cancellationToken);
        return users.Select(u => u.ToDto());
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        return user?.ToDto();
    }

    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalized = email.ToLowerInvariant();
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email.ToLower() == normalized, cancellationToken);
        return user?.ToDto();
    }

    public async Task<UserDto> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var existing = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (existing)
        {
            throw new ArgumentException("Email already in use", nameof(request.Email));
        }

        var entity = new ApplicationUser
        {
            Email = request.Email,
            Name = request.Name,
            Username = request.Username,
            Role = request.Role,
            Avatar = request.Avatar,
            IsActive = request.IsActive,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<UserDto?> UpdateAsync(Guid id, UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (entity == null)
        {
            return null;
        }

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            entity.Name = request.Name;
        }

        if (!string.IsNullOrWhiteSpace(request.Username))
        {
            entity.Username = request.Username;
        }

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        }

        if (request.Role.HasValue)
        {
            entity.Role = request.Role.Value;
        }

        if (request.Avatar != null)
        {
            entity.Avatar = request.Avatar;
        }

        if (request.IsActive.HasValue)
        {
            entity.IsActive = request.IsActive.Value;
        }

        entity.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return entity.ToDto();
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        if (entity == null)
        {
            return false;
        }

        _context.Users.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
