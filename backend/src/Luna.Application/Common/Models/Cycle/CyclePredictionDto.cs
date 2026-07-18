namespace Luna.Application.Common.Models.Cycle;

public record CyclePredictionDto
{
    public string CurrentPhase { get; set; } = "unknown";

    public int? DayOfCycle { get; set; }

    public DateOnly? NextPeriodStart { get; set; }

    public DateOnly? NextPeriodEnd { get; set; }

    public DateOnly? FertileWindowStart { get; set; }

    public DateOnly? FertileWindowEnd { get; set; }

    public DateOnly? OvulationDate { get; set; }

    public int? DaysUntilNextPeriod { get; set; }
}
