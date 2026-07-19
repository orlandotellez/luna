namespace Luna.Application.Common.Models.Cycle;

public record SymptomEntryDto
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public string Symptom { get; set; } = string.Empty;
    public int? Severity { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}
