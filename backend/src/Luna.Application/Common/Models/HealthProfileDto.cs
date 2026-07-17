namespace Luna.Application.Common.Models;

public record HealthProfileDto
{
    public bool? HasRegularCycle { get; set; }
    public int? CycleLengthDays { get; set; }
    public int? PeriodLengthDays { get; set; }
    public bool HasEndometriosis { get; set; }
    public bool HasPcos { get; set; }
    public bool HasThyroidIssues { get; set; }
    public bool HasGestationalDiabetes { get; set; }
    public bool HasFibroids { get; set; }
    public bool HasHypertension { get; set; }
    public string? Allergies { get; set; }
    public List<string>? Medications { get; set; }
    public int PreviousPregnancies { get; set; }
    public string? Surgeries { get; set; }
    public List<string>? Vaccinations { get; set; }
}
