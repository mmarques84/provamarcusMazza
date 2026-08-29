using MediatR;
using provamarcusMazza.Application.Common.Exceptions;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Application.Users.Common;
using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Application.Users.Commands.CreateUser;

public sealed class CreateUserHandler(
    IUserRepository userRepository,
    IPasswordService passwordService,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateUserCommand, UserResponse>
{
    public async Task<UserResponse> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            throw new ConflictException("A user with this email is already registered.");

        var user = new User(
            Guid.NewGuid(),
            request.Email,
            passwordService.Hash(request.Password));

        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.ToResponse();
    }
}
