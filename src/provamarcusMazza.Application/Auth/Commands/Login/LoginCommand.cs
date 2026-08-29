using MediatR;

namespace provamarcusMazza.Application.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password)
    : IRequest<LoginResponse>;

public sealed record LoginResponse(string AccessToken, string TokenType = "Bearer");
