using Luna.Domain.Enums;

namespace Luna.Application.Common.Models.Cycle;

public record CycleCalendarDayDto
{
    public DateOnly Date { get; set; }
    public int? DayOfCycle { get; set; }
    public CyclePhase Phase { get; set; } = CyclePhase.Unknown;
    public bool IsPeriodDay { get; set; }
    public bool IsPredictedPeriod { get; set; }
    public bool IsFertileWindow { get; set; }
    public bool IsOvulationDay { get; set; }
    public List<SymptomEntryDto> Symptoms { get; set; } = new();
}
