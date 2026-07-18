using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Interfaces.Repositories;
using Luna.Application.Common.Models;
using Luna.Application.Common.Mapping;
using Luna.Domain.Exceptions;
using Luna.Domain.Enums;
using Luna.Domain.Entities;

namespace Luna.Application.Features.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IPregnancyRepository _pregnancyRepository;
    private readonly IHealthProfileRepository _healthProfileRepository;

    public UserService(
        IUserRepository userRepository,
        IUserProfileRepository userProfileRepository,
        IPregnancyRepository pregnancyRepository,
        IHealthProfileRepository healthProfileRepository
        )
    {
        _userRepository = userRepository;
        _userProfileRepository = userProfileRepository;
        _pregnancyRepository = pregnancyRepository;
        _healthProfileRepository = healthProfileRepository;
    }

    public async Task<UserDto> GetMyProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        user.Profile = await _userProfileRepository.GetByUserIdAsync(userId);
        user.HealthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);

        // load pregnancies
        var pregnancies = await _pregnancyRepository.GetByUserIdAsync(userId);

        return user.MapUserToDto();
    }

    public async Task<UserDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        var profile = await _userProfileRepository.GetByUserIdAsync(userId);
        if (profile is null)
        {
            profile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _userProfileRepository.CreateAsync(profile);
        }


        if (request.UserName is not null && !string.Equals(request.UserName, profile.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var existingProfile = await _userProfileRepository.GetByUserNameAsync(request.UserName);

            if (existingProfile is not null) throw AppExceptions.Conflict("UserName is already taken");

            profile.UserName = request.UserName;
        }

        if (request.Name is not null) user.Name = request.Name;
        if (request.Phone is not null) user.Phone = request.Phone;
        if (request.Bio is not null) profile.Bio = request.Bio;

        user.UpdatedAt = DateTime.UtcNow;
        profile.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        await _userProfileRepository.UpdateAsync(profile);

        return user.MapUserToDto();
    }

    public async Task<UserDto> UpdateLifeStageAsync(Guid userId, UpdateLifeStageRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        user.LifeStage = request.LifeStage;

        if (request.LifeStage == LifeStage.Pregnancy)
        {
            // Buscar embarazo activo o crear uno nuevo
            var pregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);

            if (pregnancy is null)
            {
                pregnancy = new Pregnancy
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    LastMenstrualPeriod = request.LastMenstrualPeriod,
                    EstimatedDueDate = request.EstimatedDueDate!.Value,
                    CurrentWeek = CalculateCurrentWeek(request.EstimatedDueDate!.Value),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _pregnancyRepository.CreateAsync(pregnancy);
            }
            else
            {
                pregnancy.LastMenstrualPeriod = request.LastMenstrualPeriod ?? pregnancy.LastMenstrualPeriod;
                pregnancy.EstimatedDueDate = request.EstimatedDueDate ?? pregnancy.EstimatedDueDate;
                pregnancy.UpdatedAt = DateTime.UtcNow;
                await _pregnancyRepository.UpdateAsync(pregnancy);
            }
        }
        else
        {
            // Si cambia a otra etapa, desactivar embarazo activo
            var activePregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);
            if (activePregnancy is not null)
            {
                activePregnancy.IsActive = false;
                activePregnancy.EndedAt = DateTime.UtcNow;
                activePregnancy.UpdatedAt = DateTime.UtcNow;
                await _pregnancyRepository.UpdateAsync(activePregnancy);
            }
        }

        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Cargar relaciones para el DTO
        user.Profile = await _userProfileRepository.GetByUserIdAsync(userId);
        user.HealthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);

        var result = user.MapUserToDto();
        return result;
    }

    public async Task<HealthProfileDto?> GetHealthProfileAsync(Guid userId)
    {
        var healthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);

        return healthProfile.MapHealthProfileToDto();
    }

    private static int CalculateCurrentWeek(DateOnly estimatedDueDate)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var daysUntilDue = estimatedDueDate.DayNumber - today.DayNumber;
        var weeks = (280 - daysUntilDue) / 7; // 280 días = 40 semanas
        return Math.Clamp(weeks, 0, 42);
    }
}


