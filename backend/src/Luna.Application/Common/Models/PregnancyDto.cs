namespace Luna.Application.Common.Models;

public record PregnancyDto
{
    public Guid Id { get; set; }
    public DateOnly? LastMenstrualPeriod { get; set; }
    public DateOnly EstimatedDueDate { get; set; }
    public int CurrentWeek { get; set; }
    public bool? IsFirstPregnancy { get; set; }
    public int PregnancyCount { get; set; }
    public bool IsActive { get; set; }
    public DateTime? EndedAt { get; set; }
    public string? Notes { get; set; }
}
