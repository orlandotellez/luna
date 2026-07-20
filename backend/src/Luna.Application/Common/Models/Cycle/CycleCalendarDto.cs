using Luna.Domain.Enums;

namespace Luna.Application.Common.Models.Cycle;

public record CycleCalendarDto
{
    public Guid UserId { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public LifeStage LifeStage { get; set; }
    public List<CycleCalendarDayDto> Days { get; set; } = new();
}
