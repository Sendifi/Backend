using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using BackendSendify.Users.Domain.Model.Aggregates;

namespace BackendSendify.Users.Application.Internal.InMemory;

internal static class InMemoryDataStore
{
    public static readonly ConcurrentDictionary<Guid, User> Users = new();

    static InMemoryDataStore()
    {
        var admin = new User(
            username: "admin",
            email: "admin@sendify.com",
            name: "Administrador",
            role: "ADMIN",
            avatar: "https://example.com/avatar-admin.jpg",
            isActive: true,
            passwordHash: Sha256("admin123"));

        var support = new User(
            username: "support",
            email: "support@sendify.com",
            name: "Soporte",
            role: "ANALYST",
            avatar: "https://example.com/avatar-support.jpg",
            isActive: true,
            passwordHash: Sha256("support123"));

        Users[admin.Id] = admin;
        Users[support.Id] = support;
    }

    internal static string Sha256(string value)
    {
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }
}

