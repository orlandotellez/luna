using Luna.Domain.Enums;

namespace Luna.Application.Common.Models.Cycle;

public record CycleForecastResponseDto
{
    public Guid UserId { get; set; }
    public LifeStage LifeStage { get; set; }
    public string LifeStageState { get; set; } = "active_cycle";
    public bool IsPredictionApplicable { get; set; } = true;

    public CyclePredictionDto? Prediction { get; set; }
    public CycleForecastMetadataDto? Metadata { get; set; }
}

public record CycleForecastMetadataDto
{
    public string Confidence { get; set; } = "low";
    public string Source { get; set; } = "default";
    public int AssumedCycleLength { get; set; }
    public int AssumedPeriodLength { get; set; }
    public int CyclesAnalyzed { get; set; }
    public bool IsAnomaly { get; set; }
}
