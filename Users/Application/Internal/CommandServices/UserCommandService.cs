using System.Text.RegularExpressions;
using BackendSendify.Shared.Domain.Repositories;
using BackendSendify.Users.Application.Internal.InMemory;
using BackendSendify.Users.Domain.Commands;
using BackendSendify.Users.Domain.Model.Aggregates;
using BackendSendify.Users.Domain.Repositories;
using BackendSendify.Users.Domain.Services;

namespace BackendSendify.Users.Application.Internal.CommandServices;

public class UserCommandService(IUserRepository userRepository, IUnitOfWork unitOfWork) : IUserCommandService
{
    private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
    {
        "ADMIN","ANALYST","OPERATOR","VIEWER","DELIVERY"
    };

    public async Task<(bool Success, string? Error, User? User)> Handle(CreateUserCommand command, CancellationToken ct = default)
    {
        if (!IsValidEmail(command.Email)) return (false, "Invalid email", null);
        if (await userRepository.AnyByEmailAsync(command.Email, ct)) return (false, "Email already in use", null);
        if (await userRepository.AnyByUsernameAsync(command.Username, ct)) return (false, "Username already in use", null);

        var normalizedRole = NormalizeRole(command.Role);
        if (!AllowedRoles.Contains(normalizedRole)) return (false, "Invalid role", null);

        var passwordHash = PasswordHashing.Hash(command.Password);

        var user = new User(
            username: command.Username,
            email: command.Email,
            name: command.Name,
            role: normalizedRole,
            avatar: command.Avatar,
            isActive: command.IsActive,
            passwordHash: passwordHash);

        await userRepository.AddAsync(user, ct);
        await unitOfWork.CompleteAsync(ct);
        return (true, null, user);
    }

    public async Task<(bool Success, string? Error, User? User)> Handle(UpdateUserCommand command, CancellationToken ct = default)
    {
        var user = await userRepository.FindByIdAsync(command.Id, ct);
        if (user is null) return (false, "User not found", null);

        if (!string.IsNullOrWhiteSpace(command.Email))
        {
            if (!IsValidEmail(command.Email)) return (false, "Invalid email", null);
            var existingByEmail = await userRepository.FindByEmailAsync(command.Email, ct);
            if (existingByEmail is not null && existingByEmail.Id != user.Id)
                return (false, "Email already in use", null);
        }

        if (!string.IsNullOrWhiteSpace(command.Username))
        {
            var existingByUsername = await userRepository.FindByUsernameAsync(command.Username, ct);
            if (existingByUsername is not null && existingByUsername.Id != user.Id)
                return (false, "Username already in use", null);
        }

        var normalizedRole = !string.IsNullOrWhiteSpace(command.Role) ? NormalizeRole(command.Role!) : null;
        if (normalizedRole is not null && !AllowedRoles.Contains(normalizedRole))
            return (false, "Invalid role", null);

        user.Update(
            username: command.Username,
            email: command.Email,
            name: command.Name,
            role: normalizedRole,
            avatar: command.Avatar,
            isActive: command.IsActive);

        userRepository.Update(user);
        await unitOfWork.CompleteAsync(ct);
        return (true, null, user);
    }

    public async Task<(bool Success, string? Error)> Handle(DeleteUserCommand command, CancellationToken ct = default)
    {
        var user = await userRepository.FindByIdAsync(command.Id, ct);
        if (user is null) return (false, "User not found");
        userRepository.Remove(user);
        await unitOfWork.CompleteAsync(ct);
        return (true, null);
    }

    private static bool IsValidEmail(string email)
        => Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

    private static string NormalizeRole(string role) => role.Trim().ToUpperInvariant();
}

