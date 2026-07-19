using Luna.Application.Common.Models.User;
using Luna.Application.Common.Models.Cycle;

namespace Luna.Application.Common.Interfaces.Services;

public interface IUserService
{
    Task<UserDto> GetMyProfileAsync(Guid userId);
    Task<UserDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request);
    Task<UserDto> UpdateLifeStageAsync(Guid userId, UpdateLifeStageRequest request);
    Task<HealthProfileDto?> GetHealthProfileAsync(Guid userId);
    Task<HealthProfileDto> UpdateHealthProfileAsync(Guid userId, UpdateHealthProfileRequest request);
    Task<UserDto> UpdateAvatarAsync(Guid userId, string imageUrl);
    Task<CycleCurrentDto> GetCurrentCycleAsync(Guid userId);
    Task<PeriodEntryDto> RegisterPeriodAsync(Guid userId, RegisterPeriodRequest request);
    Task<SymptomEntryDto> RegisterSymptomAsync(Guid userId, RegisterSymptomRequest request);
}
