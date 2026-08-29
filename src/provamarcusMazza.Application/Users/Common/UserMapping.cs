using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Application.Users.Common;

internal static class UserMapping
{
    public static UserResponse ToResponse(this User user)
        => new(user.Id, user.Email, user.IsActive, user.CreatedAt);
}
