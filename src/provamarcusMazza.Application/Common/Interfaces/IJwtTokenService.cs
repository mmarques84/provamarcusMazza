using provamarcusMazza.Domain.Entities;

namespace provamarcusMazza.Application.Common.Interfaces;

public interface IJwtTokenService
{
    string Generate(User user);
}
