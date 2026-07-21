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

    public async Task<CycleStatsDto> GetStatsAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        var lifeStage = user.LifeStage ?? LifeStage.ActiveCycle;
        var activePregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);
        if (activePregnancy is not null)
            lifeStage = LifeStage.Pregnancy;

        if (lifeStage == LifeStage.Pregnancy)
        {
            return new CycleStatsDto
            {
                UserId = userId,
                LifeStage = lifeStage,
                TotalCyclesTracked = 0,
                CyclesAnalyzed = 0,
                InsufficientData = true
            };
        }

        var healthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);
        var cycleLengthDefault = healthProfile?.CycleLengthDays ?? 28;
        var periodLengthDefault = healthProfile?.PeriodLengthDays ?? 5;

        var allPeriods = await _periodEntryRepository.GetByUserIdAsync(userId);
        var ascending = allPeriods.OrderBy(p => p.StartDate).ToList();

        var enriched = new List<(PeriodEntry period, int? cycleLength, int? periodLength, bool inProgress)>();
        for (var i = 0; i < ascending.Count; i++)
        {
            var current = ascending[i];

            int? cycleLength = null;
            if (i > 0)
                cycleLength = current.StartDate.DayNumber - ascending[i - 1].StartDate.DayNumber;

            var inProgress = !current.EndDate.HasValue;
            int? periodLength = null;
            if (current.EndDate.HasValue)
                periodLength = current.EndDate.Value.DayNumber - current.StartDate.DayNumber + 1;

            enriched.Add((current, cycleLength, periodLength, inProgress));
        }

        var completedCycleLengths = enriched
            .Where(x => !x.inProgress && x.cycleLength.HasValue)
            .Select(x => (double)x.cycleLength!.Value)
            .ToList();

        var completedPeriodLengths = enriched
            .Where(x => !x.inProgress && x.periodLength.HasValue)
            .Select(x => (double)x.periodLength!.Value)
            .ToList();

        var totalTracked = allPeriods.Count;
        var cyclesAnalyzed = completedCycleLengths.Count;
        var insufficientData = cyclesAnalyzed < 3;

        var regularity = ComputeRegularity(completedCycleLengths, cyclesAnalyzed);
        var periodLengthStats = ComputePeriodLength(completedPeriodLengths);
        var trends = ComputeTrends(completedCycleLengths, completedPeriodLengths);

        CycleLastCycleStatsDto? lastCycleStats = null;
        var lastEntry = ascending.LastOrDefault();
        if (lastEntry != null)
        {
            var daysSinceStart = DateOnly.FromDateTime(DateTime.UtcNow).DayNumber - lastEntry.StartDate.DayNumber;
            lastCycleStats = new CycleLastCycleStatsDto
            {
                Id = lastEntry.Id,
                StartDate = lastEntry.StartDate,
                EndDate = lastEntry.EndDate,
                DaysSinceStart = daysSinceStart
            };
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var rangeStart = today.AddYears(-5);
        var symptoms = await _symptomEntryRepository.GetByUserIdInRangeAsync(userId, rangeStart, today);
        var topSymptoms = symptoms
            .GroupBy(s => s.Symptom)
            .Select(g => new CycleSymptomFrequencyDto
            {
                Symptom = g.Key,
                Occurrences = g.Count(),
                AverageSeverity = g.Any(x => x.Severity.HasValue)
                    ? Math.Round(g.Where(x => x.Severity.HasValue).Average(x => (double)x.Severity!.Value), 1)
                    : (double?)null
            })
            .OrderByDescending(x => x.Occurrences)
            .Take(5)
            .ToList();

        var predictionBaseline = ComputePredictionBaseline(lastEntry, cycleLengthDefault, periodLengthDefault, cyclesAnalyzed);

        return new CycleStatsDto
        {
            UserId = userId,
            LifeStage = lifeStage,
            TotalCyclesTracked = totalTracked,
            CyclesAnalyzed = cyclesAnalyzed,
            InsufficientData = insufficientData,
            Regularity = regularity,
            PeriodLength = periodLengthStats,
            Trends = trends,
            LastCycle = lastCycleStats,
            TopSymptoms = topSymptoms,
            PredictionBaseline = predictionBaseline
        };
    }

    private static CycleRegularityStatsDto ComputeRegularity(List<double> cycleLengths, int cyclesAnalyzed)
    {
        if (cycleLengths.Count == 0)
            return new CycleRegularityStatsDto();

        var avg = cycleLengths.Average();
        var sorted = cycleLengths.OrderBy(x => x).ToList();
        var median = sorted[sorted.Count / 2];
        var min = sorted.First();
        var max = sorted.Last();
        var variance = cycleLengths.Sum(x => Math.Pow(x - avg, 2)) / cycleLengths.Count;
        var stdDev = Math.Sqrt(variance);

        var withinThreshold = cycleLengths.Count(x => Math.Abs(x - avg) <= 3);
        var score = cyclesAnalyzed > 0 ? (int)Math.Round(withinThreshold * 100.0 / cyclesAnalyzed) : 0;
        var isRegular = cyclesAnalyzed >= 3 && score >= 80;

        return new CycleRegularityStatsDto
        {
            IsRegular = isRegular,
            RegularityScore = score,
            AverageCycleLengthDays = Math.Round(avg, 1),
            MedianCycleLengthDays = (int)median,
            MinCycleLengthDays = (int)min,
            MaxCycleLengthDays = (int)max,
            StdDeviationDays = Math.Round(stdDev, 1)
        };
    }

    private static CyclePeriodLengthStatsDto ComputePeriodLength(List<double> periodLengths)
    {
        if (periodLengths.Count == 0)
            return new CyclePeriodLengthStatsDto();

        return new CyclePeriodLengthStatsDto
        {
            AveragePeriodLengthDays = Math.Round(periodLengths.Average(), 1),
            MinPeriodLengthDays = (int)periodLengths.Min(),
            MaxPeriodLengthDays = (int)periodLengths.Max()
        };
    }

    private static CycleTrendStatsDto ComputeTrends(List<double> cycleLengths, List<double> periodLengths)
    {
        var trendResult = new CycleTrendStatsDto();

        if (cycleLengths.Count >= 4)
        {
            var midpoint = cycleLengths.Count / 2;
            var firstHalf = cycleLengths.Take(midpoint).Average();
            var secondHalf = cycleLengths.Skip(midpoint).Average();
            var diff = secondHalf - firstHalf;
            trendResult.CycleLengthChangeLastCycles = Math.Round(diff, 1);
            trendResult.CycleLengthTrend = Math.Abs(diff) < 1 ? "stable"
                : diff > 0 ? "increasing"
                : "decreasing";
        }

        if (periodLengths.Count >= 4)
        {
            var midpoint = periodLengths.Count / 2;
            var firstHalf = periodLengths.Take(midpoint).Average();
            var secondHalf = periodLengths.Skip(midpoint).Average();
            var diff = secondHalf - firstHalf;
            trendResult.PeriodLengthChangeLastCycles = Math.Round(diff, 1);
            trendResult.PeriodLengthTrend = Math.Abs(diff) < 1 ? "stable"
                : diff > 0 ? "increasing"
                : "decreasing";
        }

        return trendResult;
    }

    private static CyclePredictionBaselineDto ComputePredictionBaseline(
        PeriodEntry? lastEntry,
        int cycleLength,
        int periodLength,
        int cyclesAnalyzed)
    {
        var baseline = new CyclePredictionBaselineDto();

        if (lastEntry == null)
        {
            baseline.Confidence = "low";
            return baseline;
        }

        var nextStart = lastEntry.StartDate.AddDays(cycleLength);
        var nextEnd = nextStart.AddDays(periodLength - 1);
        var ovulationDayOffset = cycleLength - 14;
        var ovulation = lastEntry.StartDate.AddDays(ovulationDayOffset - 1);
        var fertileStart = ovulation.AddDays(-5);
        var fertileEnd = ovulation.AddDays(1);

        baseline.NextPeriodEstimatedStart = nextStart;
        baseline.NextPeriodEstimatedEnd = nextEnd;
        baseline.OvulationDate = ovulation;
        baseline.FertilityWindowStart = fertileStart;
        baseline.FertilityWindowEnd = fertileEnd;
        baseline.Confidence = cyclesAnalyzed >= 6 ? "high"
            : cyclesAnalyzed >= 3 ? "medium"
            : "low";

        return baseline;
    }

    public async Task<CycleForecastResponseDto> GetPredictionsAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        var lifeStage = user.LifeStage ?? LifeStage.ActiveCycle;
        var activePregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);
        if (activePregnancy is not null)
            lifeStage = LifeStage.Pregnancy;

        var lifeStageState = lifeStage switch
        {
            LifeStage.Pregnancy => "pregnancy",
            LifeStage.Menopause => "menopause",
            LifeStage.Adolescent => "adolescent",
            _ => "active_cycle"
        };

        if (lifeStage == LifeStage.Pregnancy || lifeStage == LifeStage.Menopause)
        {
            return new CycleForecastResponseDto
            {
                UserId = userId,
                LifeStage = lifeStage,
                LifeStageState = lifeStageState,
                IsPredictionApplicable = false,
                Prediction = null,
                Metadata = null
            };
        }

        var healthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);
        var allPeriods = (await _periodEntryRepository.GetByUserIdAsync(userId))
            .OrderBy(p => p.StartDate)
            .ToList();

        var cycleLength = 28;
        var periodLength = 5;
        var source = "default";
        var cyclesAnalyzed = 0;

        if (healthProfile?.HasRegularCycle == true && healthProfile.CycleLengthDays.HasValue)
        {
            cycleLength = healthProfile.CycleLengthDays.Value;
            periodLength = healthProfile.PeriodLengthDays ?? 5;
            source = "config";
        }
        else
        {
            var cycleLengthsHistory = new List<int>();
            var periodLengthsHistory = new List<int>();

            for (var i = 1; i < allPeriods.Count; i++)
            {
                var prev = allPeriods[i - 1];
                var curr = allPeriods[i];
                if (prev.EndDate.HasValue)
                {
                    cycleLengthsHistory.Add(curr.StartDate.DayNumber - prev.StartDate.DayNumber);
                    if (curr.EndDate.HasValue)
                        periodLengthsHistory.Add(curr.EndDate.Value.DayNumber - curr.StartDate.DayNumber + 1);
                }
            }

            var recentCycles = cycleLengthsHistory.TakeLast(6).ToList();
            var recentPeriods = periodLengthsHistory.TakeLast(6).ToList();
            cyclesAnalyzed = recentCycles.Count;

            if (recentCycles.Count >= 3)
            {
                cycleLength = (int)Math.Round(Median(recentCycles), 0);
                source = "historical_median";
            }
            if (recentPeriods.Count > 0)
            {
                periodLength = (int)Math.Round(Median(recentPeriods), 0);
            }
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var prediction = ComputeCurrentAndFuture(today, allPeriods, cycleLength, periodLength, out var isAnomaly);

        var confidence = source == "config" || cyclesAnalyzed >= 6 ? "high"
            : cyclesAnalyzed >= 3 ? "medium"
            : "low";

        return new CycleForecastResponseDto
        {
            UserId = userId,
            LifeStage = lifeStage,
            LifeStageState = lifeStageState,
            IsPredictionApplicable = true,
            Prediction = prediction,
            Metadata = new CycleForecastMetadataDto
            {
                Confidence = confidence,
                Source = source,
                AssumedCycleLength = cycleLength,
                AssumedPeriodLength = periodLength,
                CyclesAnalyzed = cyclesAnalyzed,
                IsAnomaly = isAnomaly
            }
        };
    }

    private static double Median(List<int> values)
    {
        if (values.Count == 0) return 0;
        var sorted = values.OrderBy(x => x).ToList();
        var n = sorted.Count;
        return n % 2 == 1
            ? sorted[n / 2]
            : (sorted[(n / 2) - 1] + sorted[n / 2]) / 2.0;
    }

    private static CyclePredictionDto ComputeCurrentAndFuture(
        DateOnly today,
        List<PeriodEntry> allPeriodsAsc,
        int cycleLength,
        int periodLength,
        out bool isAnomaly)
    {
        isAnomaly = false;
        var prediction = new CyclePredictionDto();

        if (allPeriodsAsc.Count == 0)
        {
            prediction.CurrentPhase = "unknown";
            return prediction;
        }

        var lastEntry = allPeriodsAsc.Last();

        var nextStart = lastEntry.StartDate.AddDays(cycleLength);
        var nextStartDayNumber = nextStart.DayNumber;
        var daysUntilNext = nextStartDayNumber - today.DayNumber;

        var ovulationOffset = cycleLength - 14;
        var ovulationDate = lastEntry.StartDate.AddDays(ovulationOffset - 1);
        var fertileStart = ovulationDate.AddDays(-5);
        var fertileEnd = ovulationDate.AddDays(1);

        prediction.NextPeriodStart = nextStart;
        prediction.NextPeriodEnd = nextStart.AddDays(periodLength - 1);
        prediction.FertileWindowStart = fertileStart;
        prediction.FertileWindowEnd = fertileEnd;
        prediction.OvulationDate = ovulationDate;
        prediction.DaysUntilNextPeriod = daysUntilNext;

        var dayOfCycle = today.DayNumber - lastEntry.StartDate.DayNumber + 1;
        prediction.DayOfCycle = dayOfCycle;

        if (dayOfCycle <= periodLength)
        {
            prediction.CurrentPhase = "menstrual";
        }
        else if (dayOfCycle <= cycleLength - 16)
        {
            prediction.CurrentPhase = "follicular";
        }
        else if (dayOfCycle <= cycleLength - 13)
        {
            prediction.CurrentPhase = "ovulatory";
        }
        else if (dayOfCycle <= cycleLength)
        {
            prediction.CurrentPhase = "luteal";
        }
        else
        {
            prediction.CurrentPhase = "late";
            isAnomaly = dayOfCycle > cycleLength + 14;
        }

        return prediction;
    }
}
