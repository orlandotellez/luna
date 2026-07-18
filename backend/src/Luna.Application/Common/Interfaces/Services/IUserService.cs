using Luna.Application.Common.Models;

namespace Luna.Application.Common.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> GetMyProfileAsync(Guid userId);
    Task<UserDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request);
    Task<UserDto> UpdateLifeStageAsync(Guid userId, UpdateLifeStageRequest request);
    Task<HealthProfileDto?> GetHealthProfileAsync(Guid userId);
}
