namespace Luna.Domain.Entities;

public class Pregnancy
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateOnly? LastMenstrualPeriod { get; set; }
    public DateOnly EstimatedDueDate { get; set; }
    public int CurrentWeek { get; set; }
    public bool? IsFirstPregnancy { get; set; }
    public int PregnancyCount { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTime? EndedAt { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
