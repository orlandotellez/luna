using Luna.Application.Common.Interfaces.Repositories.Users;
using Luna.Application.Common.Interfaces.Repositories.Cycle;
using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Models.Pregnancy;
using Luna.Application.Common.Helpers;
using Luna.Domain.Enums;
using Luna.Domain.Exceptions;

namespace Luna.Application.Features.Pregnancies;

public class PregnancyService : IPregnancyService
{
    private readonly IUserRepository _userRepository;
    private readonly IPregnancyRepository _pregnancyRepository;

    public PregnancyService(
        IUserRepository userRepository,
        IPregnancyRepository pregnancyRepository)
    {
        _userRepository = userRepository;
        _pregnancyRepository = pregnancyRepository;
    }

    public async Task<PregnancyRegistrationResponseDto> RegisterPregnancyAsync(
        Guid userId,
        RegisterPregnancyRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        if (request.LastMenstrualPeriod >= today)
            throw AppExceptions.BadRequest("LastMenstrualPeriod must be in the past.");

        if (request.LastMenstrualPeriod < today.AddDays(-300))
            throw AppExceptions.BadRequest("LastMenstrualPeriod cannot be more than ~10 months ago.");

        var existingActive = await _pregnancyRepository.GetActiveByUserIdAsync(userId);
        if (existingActive is not null)
            throw AppExceptions.Conflict("User already has an active pregnancy.");

        var estimatedDueDate = request.LastMenstrualPeriod.AddDays(280);
        var currentWeek = PregnancyHelper.CalculateCurrentWeek(estimatedDueDate);

        var trimester = currentWeek switch
        {
            <= 13 => 1,
            <= 27 => 2,
            _ => 3
        };

        var weeksRemaining = Math.Max(0, 40 - currentWeek);

        var allPregnancies = await _pregnancyRepository.GetByUserIdAsync(userId);
        var pregnancyCount = allPregnancies.Count + 1;

        var pregnancy = new Luna.Domain.Entities.Pregnancies.Pregnancy
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            LastMenstrualPeriod = request.LastMenstrualPeriod,
            EstimatedDueDate = estimatedDueDate,
            CurrentWeek = currentWeek,
            IsFirstPregnancy = request.IsFirstPregnancy,
            PregnancyCount = pregnancyCount,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _pregnancyRepository.CreateAsync(pregnancy);

        user.LifeStage = LifeStage.Pregnancy;
        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        return new PregnancyRegistrationResponseDto
        {
            PregnancyId = pregnancy.Id,
            CurrentWeek = currentWeek,
            EstimatedDueDate = estimatedDueDate,
            Trimester = trimester,
            WeeksRemaining = weeksRemaining
        };
    }

    public async Task<CurrentPregnancyResponseDto> GetCurrentPregnancyAsync(Guid userId)
    {
        var pregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);
        if (pregnancy is null)
            throw AppExceptions.NotFound("No active pregnancy found for this user.");

        var currentWeek = PregnancyHelper.CalculateCurrentWeek(pregnancy.EstimatedDueDate);

        var trimester = currentWeek switch
        {
            <= 13 => 1,
            <= 27 => 2,
            _ => 3
        };

        var weeksRemaining = Math.Max(0, 40 - currentWeek);

        return new CurrentPregnancyResponseDto
        {
            PregnancyId = pregnancy.Id,
            LastMenstrualPeriod = pregnancy.LastMenstrualPeriod,
            EstimatedDueDate = pregnancy.EstimatedDueDate,
            CurrentWeek = currentWeek,
            Trimester = trimester,
            WeeksRemaining = weeksRemaining,
            IsFirstPregnancy = pregnancy.IsFirstPregnancy,
            IsActive = pregnancy.IsActive,
            Notes = pregnancy.Notes
        };
    }

    public async Task<int?> GetActivePregnancyCurrentWeekAsync(Guid userId)
    {
        var pregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);
        if (pregnancy is null)
            return null;

        return PregnancyHelper.CalculateCurrentWeek(pregnancy.EstimatedDueDate);
    }
}
