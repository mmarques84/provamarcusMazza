namespace provamarcusMazza.Application.Customers.Common;

public sealed record CustomerResponse(
    Guid Id,
    string Name,
    string Email,
    DateTime CreatedAt);
