namespace provamarcusMazza.Application.Users.Common;

public sealed record UserResponse(
    Guid Id,
    string Email,
    bool IsActive,
    DateTime CreatedAt);
