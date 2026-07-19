using Luna.Application.Common.Interfaces.Repositories;
using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Models.User;
using Luna.Application.Common.Models.Cycle;
using Luna.Application.Common.Mapping;
using Luna.Domain.Exceptions;
using Luna.Domain.Enums;
using Luna.Domain.Entities.Cycle;

namespace Luna.Application.Features.Cycle;

public class CycleService : ICycleService
{

    private readonly IUserRepository _userRepository;
    private readonly IPeriodEntryRepository _periodEntryRepository;
    private readonly IHealthProfileRepository _healthProfileRepository;
    private readonly IPregnancyRepository _pregnancyRepository;

    public CycleService(
        IUserRepository userRepository,
        IPeriodEntryRepository periodEntryRepository,
        IHealthProfileRepository healthProfileRepository,
        IPregnancyRepository pregnancyRepository

        )
    {
        _userRepository = userRepository;
        _periodEntryRepository = periodEntryRepository;
        _healthProfileRepository = healthProfileRepository;
        _pregnancyRepository = pregnancyRepository;
    }

    public async Task<CycleCurrentDto> GetCurrentCycleAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        var healthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);

        var activePregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);

        var result = new CycleCurrentDto
        {
            LifeStage = user.LifeStage ?? LifeStage.ActiveCycle,
            CycleLengthDays = healthProfile?.CycleLengthDays,
            PeriodLengthDays = healthProfile?.PeriodLengthDays,
            HasRegularCycle = healthProfile?.HasRegularCycle,
            ActivePregnancy = activePregnancy.MapPregnancyToDto(),
            Predictions = null
        };

        if (healthProfile?.HasRegularCycle == true && healthProfile.CycleLengthDays.HasValue)
        {
            result.Predictions = new CyclePredictionDto
            {
                CurrentPhase = "regular_cycle_configured",
            };
        }

        if (activePregnancy is not null)
        {
            result.LifeStage = LifeStage.Pregnancy;
        }

        return result;
    }

    public async Task<PeriodEntryDto> RegisterPeriodAsync(Guid userId, RegisterPeriodRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        if (request.EndDate.HasValue && request.EndDate.Value < request.StartDate)
            throw AppExceptions.BadRequest("EndDate cannot be before StartDate");

        var period = new PeriodEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _periodEntryRepository.CreateAsync(period);

        return period.MapPeriodEntryToDto();
    }

}



