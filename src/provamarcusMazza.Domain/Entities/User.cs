using provamarcusMazza.Domain.Common;

namespace provamarcusMazza.Domain.Entities;

public sealed class User
{
    private User() { }

    public User(Guid id, string email, string passwordHash)
    {
        if (id == Guid.Empty) throw new DomainException("User id is required.");
        if (string.IsNullOrWhiteSpace(email)) throw new DomainException("User email is required.");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new DomainException("Password hash is required.");

        Id = id;
        Email = email.Trim().ToLowerInvariant();
        PasswordHash = passwordHash;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
}
