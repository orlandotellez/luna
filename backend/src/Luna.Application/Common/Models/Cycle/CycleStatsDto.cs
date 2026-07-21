using Luna.Domain.Enums;

namespace Luna.Application.Common.Models.Cycle;

public record CycleStatsDto
{
    public Guid UserId { get; set; }
    public LifeStage LifeStage { get; set; }
    public int TotalCyclesTracked { get; set; }
    public int CyclesAnalyzed { get; set; }
    public bool InsufficientData { get; set; }

    public CycleRegularityStatsDto Regularity { get; set; } = new();
    public CyclePeriodLengthStatsDto PeriodLength { get; set; } = new();
    public CycleTrendStatsDto Trends { get; set; } = new();
    public CycleLastCycleStatsDto? LastCycle { get; set; }
    public List<CycleSymptomFrequencyDto> TopSymptoms { get; set; } = new();
    public CyclePredictionBaselineDto PredictionBaseline { get; set; } = new();
}

public record CycleRegularityStatsDto
{
    public bool IsRegular { get; set; }
    public int RegularityScore { get; set; }
    public double? AverageCycleLengthDays { get; set; }
    public int? MedianCycleLengthDays { get; set; }
    public int? MinCycleLengthDays { get; set; }
    public int? MaxCycleLengthDays { get; set; }
    public double? StdDeviationDays { get; set; }
}

public record CyclePeriodLengthStatsDto
{
    public double? AveragePeriodLengthDays { get; set; }
    public int? MinPeriodLengthDays { get; set; }
    public int? MaxPeriodLengthDays { get; set; }
}

public record CycleTrendStatsDto
{
    public string CycleLengthTrend { get; set; } = "insufficient_data";
    public string PeriodLengthTrend { get; set; } = "insufficient_data";
    public double? CycleLengthChangeLastCycles { get; set; }
    public double? PeriodLengthChangeLastCycles { get; set; }
}

public record CycleLastCycleStatsDto
{
    public Guid Id { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int DaysSinceStart { get; set; }
    public bool IsInProgress => EndDate == null;
}

public record CycleSymptomFrequencyDto
{
    public string Symptom { get; set; } = string.Empty;
    public int Occurrences { get; set; }
    public double? AverageSeverity { get; set; }
}

public record CyclePredictionBaselineDto
{
    public DateOnly? NextPeriodEstimatedStart { get; set; }
    public DateOnly? NextPeriodEstimatedEnd { get; set; }
    public DateOnly? FertilityWindowStart { get; set; }
    public DateOnly? FertilityWindowEnd { get; set; }
    public DateOnly? OvulationDate { get; set; }
    public string Confidence { get; set; } = "low";
}
