using Luna.Domain.Entities.Users;

namespace Luna.Domain.Entities.Cycle;

public class SymptomEntry
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public DateOnly Date { get; set; }
    public string Symptom { get; set; } = string.Empty;
    public int? Severity { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
