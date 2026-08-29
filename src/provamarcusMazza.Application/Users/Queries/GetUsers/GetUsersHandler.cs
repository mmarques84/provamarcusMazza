using MediatR;
using provamarcusMazza.Application.Common.Interfaces;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Users.Common;

namespace provamarcusMazza.Application.Users.Queries.GetUsers;

public sealed class GetUsersHandler(IUserRepository userRepository)
    : IRequestHandler<GetUsersQuery, PagedResult<UserResponse>>
{
    public async Task<PagedResult<UserResponse>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var (users, totalCount) = await userRepository.GetPagedAsync(
            request.Page, request.PageSize, cancellationToken);

        return new PagedResult<UserResponse>(
            users.Select(u => u.ToResponse()).ToList(),
            request.Page,
            request.PageSize,
            totalCount);
    }
}
