namespace Luna.Application.Common.Models.Pregnancy;

public record CurrentPregnancyResponseDto
{
    public Guid PregnancyId { get; set; }
    public DateOnly? LastMenstrualPeriod { get; set; }
    public DateOnly EstimatedDueDate { get; set; }
    public int CurrentWeek { get; set; }
    public int Trimester { get; set; }
    public int WeeksRemaining { get; set; }
    public bool? IsFirstPregnancy { get; set; }
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
}
