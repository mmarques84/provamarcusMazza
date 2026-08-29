using MediatR;
using provamarcusMazza.Application.Users.Common;

namespace provamarcusMazza.Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Email,
    string Password)
    : IRequest<UserResponse>;
