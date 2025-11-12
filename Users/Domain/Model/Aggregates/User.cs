namespace BackendSendify.Users.Domain.Model.Aggregates;

public class User
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Name { get; private set; }
    public string Role { get; private set; } = "VIEWER";
    public string? Avatar { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public string PasswordHash { get; private set; } = string.Empty;

    public User(string username, string email, string? name, string role, string? avatar, bool isActive, string passwordHash)
    {
        Username = username;
        Email = email;
        Name = name;
        Role = role;
        Avatar = avatar;
        IsActive = isActive;
        PasswordHash = passwordHash;
    }

    public void Update(string? username = null, string? email = null, string? name = null, string? role = null, string? avatar = null, bool? isActive = null)
    {
        if (!string.IsNullOrWhiteSpace(username)) Username = username;
        if (!string.IsNullOrWhiteSpace(email)) Email = email;
        if (name != null) Name = name;
        if (!string.IsNullOrWhiteSpace(role)) Role = role!;
        if (avatar != null) Avatar = avatar;
        if (isActive.HasValue) IsActive = isActive.Value;
        UpdatedAt = DateTime.UtcNow;
    }
}

