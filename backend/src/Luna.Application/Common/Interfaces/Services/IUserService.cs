using Luna.Application.Common.Models;

namespace Luna.Application.Common.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> GetMyProfileAsync(Guid userId);
}
