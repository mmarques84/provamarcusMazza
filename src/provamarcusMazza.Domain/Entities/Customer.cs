using provamarcusMazza.Domain.Common;

namespace provamarcusMazza.Domain.Entities;

public sealed class Customer
{
    private Customer() { }

    public Customer(Guid id, string name, string email)
    {
        if (id == Guid.Empty) throw new DomainException("Customer id is required.");
        if (string.IsNullOrWhiteSpace(name)) throw new DomainException("Customer name is required.");
        if (string.IsNullOrWhiteSpace(email)) throw new DomainException("Customer email is required.");

        Id = id;
        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();
        CreatedAt = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
}
