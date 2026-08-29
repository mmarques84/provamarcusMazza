using MediatR;
using provamarcusMazza.Application.Common.Models;
using provamarcusMazza.Application.Users.Common;

namespace provamarcusMazza.Application.Users.Queries.GetUsers;

public sealed record GetUsersQuery(int Page = 1, int PageSize = 10)
    : IRequest<PagedResult<UserResponse>>;
