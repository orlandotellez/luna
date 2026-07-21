using Luna.Application.Common.Interfaces.Repositories.Users;
using Luna.Application.Common.Interfaces.Repositories.Cycle;
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
    private readonly ISymptomEntryRepository _symptomEntryRepository;

    public CycleService(
        IUserRepository userRepository,
        IPeriodEntryRepository periodEntryRepository,
        IHealthProfileRepository healthProfileRepository,
        IPregnancyRepository pregnancyRepository,
        ISymptomEntryRepository symptomEntryRepository

        )
    {
        _userRepository = userRepository;
        _periodEntryRepository = periodEntryRepository;
        _healthProfileRepository = healthProfileRepository;
        _pregnancyRepository = pregnancyRepository;
        _symptomEntryRepository = symptomEntryRepository;
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

    public async Task<CycleCalendarDto> GetCalendarAsync(Guid userId, int month, int year)
    {
        if (month < 1 || month > 12)
            throw AppExceptions.BadRequest("Month must be between 1 and 12.");
        if (year < 1900 || year > 2100)
            throw AppExceptions.BadRequest("Year must be between 1900 and 2100.");

        var startDate = new DateOnly(year, month, 1);
        var endDate = startDate.AddMonths(1).AddDays(-1);

        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        var lifeStage = user.LifeStage ?? LifeStage.ActiveCycle;
        var activePregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);
        if (activePregnancy is not null)
            lifeStage = LifeStage.Pregnancy;

        var healthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);
        var hasPredictionConfig = healthProfile?.HasRegularCycle == true
                                  && healthProfile.CycleLengthDays.HasValue;
        var cycleLength = healthProfile?.CycleLengthDays ?? 28;
        var periodLength = healthProfile?.PeriodLengthDays ?? 5;

        var periodsInRange = await _periodEntryRepository
            .GetOverlappingWithRangeAsync(userId, startDate, endDate);
        var symptomsInRange = await _symptomEntryRepository
            .GetByUserIdInRangeAsync(userId, startDate, endDate);

        var defaultAnchor = periodsInRange
            .Where(p => p.StartDate <= endDate)
            .OrderByDescending(p => p.StartDate)
            .FirstOrDefault();

        var days = new List<CycleCalendarDayDto>();
        for (var d = startDate; d <= endDate; d = d.AddDays(1))
        {
            var dayEntry = new CycleCalendarDayDto { Date = d };

            dayEntry.IsPeriodDay = periodsInRange.Any(p =>
                p.StartDate <= d && (p.EndDate == null || p.EndDate >= d));

            dayEntry.Symptoms = symptomsInRange
                .Where(s => s.Date == d)
                .Select(s => s.MapSymptomEntryToDto())
                .ToList();

            if (defaultAnchor != null)
            {
                var specificAnchor = periodsInRange
                    .Where(p => p.StartDate <= d)
                    .OrderByDescending(p => p.StartDate)
                    .FirstOrDefault() ?? defaultAnchor;

                var dayOfCycle = d.DayNumber - specificAnchor.StartDate.DayNumber + 1;

                if (dayOfCycle >= 1 && dayOfCycle <= cycleLength)
                {
                    dayEntry.DayOfCycle = dayOfCycle;
                    dayEntry.Phase = dayOfCycle switch
                    {
                        var n when n <= periodLength => CyclePhase.Menstrual,
                        var n when n <= cycleLength - 16 => CyclePhase.Follicular,
                        var n when n <= cycleLength - 13 => CyclePhase.Ovulatory,
                        _ => CyclePhase.Luteal
                    };

                    if (hasPredictionConfig)
                    {
                        var ovulationDay = cycleLength - 14;
                        var fertileStartDay = ovulationDay - 5;
                        var fertileEndDay = ovulationDay + 1;

                        dayEntry.IsOvulationDay = dayOfCycle == ovulationDay;
                        dayEntry.IsFertileWindow = dayOfCycle >= fertileStartDay
                                                    && dayOfCycle <= fertileEndDay;

                        if (!dayEntry.IsPeriodDay && dayOfCycle <= periodLength)
                            dayEntry.IsPredictedPeriod = true;
                    }
                }
            }

            days.Add(dayEntry);
        }

        return new CycleCalendarDto
        {
            UserId = userId,
            Month = month,
            Year = year,
            StartDate = startDate,
            EndDate = endDate,
            LifeStage = lifeStage,
            Days = days
        };
    }

    public async Task<CycleHistoryDto> GetHistoryAsync(Guid userId, int limit = 12)
    {
        if (limit <= 0 || limit > 100)
            throw AppExceptions.BadRequest("Limit must be between 1 and 100.");

        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        var lifeStage = user.LifeStage ?? LifeStage.ActiveCycle;
        var activePregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);
        if (activePregnancy is not null)
            lifeStage = LifeStage.Pregnancy;

        var allPeriods = await _periodEntryRepository.GetByUserIdAsync(userId);

        var ascending = allPeriods.OrderBy(p => p.StartDate).ToList();

        var items = new List<CycleHistoryItemDto>();
        for (var i = 0; i < ascending.Count; i++)
        {
            var current = ascending[i];

            int? cycleLengthDays = null;
            if (i > 0)
            {
                var previous = ascending[i - 1];
                cycleLengthDays = current.StartDate.DayNumber - previous.StartDate.DayNumber;
            }

            int? periodLengthDays = null;
            if (current.EndDate.HasValue)
                periodLengthDays = current.EndDate.Value.DayNumber - current.StartDate.DayNumber + 1;

            items.Add(new CycleHistoryItemDto
            {
                Id = current.Id,
                CycleNumber = i + 1,
                StartDate = current.StartDate,
                EndDate = current.EndDate,
                PeriodLengthDays = periodLengthDays,
                CycleLengthDays = cycleLengthDays,
                Notes = current.Notes,
                CreatedAt = current.CreatedAt
            });
        }

        var orderedDescending = items.OrderByDescending(x => x.StartDate).Take(limit).ToList();

        var cycleLengths = items.Where(x => x.CycleLengthDays.HasValue).Select(x => (double)x.CycleLengthDays!.Value).ToList();
        var periodLengths = items.Where(x => x.PeriodLengthDays.HasValue).Select(x => (double)x.PeriodLengthDays!.Value).ToList();

        return new CycleHistoryDto
        {
            UserId = userId,
            LifeStage = lifeStage,
            TotalCycles = allPeriods.Count,
            AverageCycleLengthDays = cycleLengths.Count > 0 ? Math.Round(cycleLengths.Average(), 1) : (double?)null,
            AveragePeriodLengthDays = periodLengths.Count > 0 ? Math.Round(periodLengths.Average(), 1) : (double?)null,
            Cycles = orderedDescending
        };
    }
}
