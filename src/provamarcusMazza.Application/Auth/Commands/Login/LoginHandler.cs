using MediatR;
using provamarcusMazza.Application.Common.Exceptions;
using provamarcusMazza.Application.Common.Interfaces;

namespace provamarcusMazza.Application.Auth.Commands.Login;

public sealed class LoginHandler(
    IUserRepository userRepository,
    IPasswordService passwordService,
    IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !user.IsActive || !passwordService.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid email or password.");

        return new LoginResponse(jwtTokenService.Generate(user));
    }
}
