using Luna.Domain.Entities.Users;

namespace Luna.Domain.Entities.Cycle;

public class PeriodEntry
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
