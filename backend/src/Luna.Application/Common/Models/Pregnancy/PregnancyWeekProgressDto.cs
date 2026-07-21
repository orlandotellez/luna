namespace Luna.Application.Common.Models.Pregnancy;

public record PregnancyWeekProgressDto
{
    public int WeekNumber { get; init; }
    public int Trimester { get; init; }
    public string Title { get; init; } = string.Empty;
    public bool IsCompleted { get; init; }
    public bool IsCurrent { get; init; }
}
