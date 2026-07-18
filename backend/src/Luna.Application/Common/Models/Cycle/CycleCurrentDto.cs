using Luna.Domain.Enums;
using Luna.Application.Common.Models.User;

namespace Luna.Application.Common.Models.Cycle;

public record CycleCurrentDto
{
    public LifeStage LifeStage { get; set; }

    public int? CycleLengthDays { get; set; }
    public int? PeriodLengthDays { get; set; }
    public bool? HasRegularCycle { get; set; }

    public PregnancyDto? ActivePregnancy { get; set; }

    public CyclePredictionDto? Predictions { get; set; }
}

