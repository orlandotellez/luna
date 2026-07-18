namespace Luna.Domain.Entities;

public class HealthProfile
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public bool? HasRegularCycle { get; set; }
    public int? CycleLengthDays { get; set; }
    public int? PeriodLengthDays { get; set; }
    public bool? HasEndometriosis { get; set; }
    public bool? HasPcos { get; set; }
    public bool? HasThyroidIssues { get; set; }
    public bool? HasGestationalDiabetes { get; set; }
    public bool? HasFibroids { get; set; }
    public bool? HasHypertension { get; set; }
    public string? Allergies { get; set; }
    public List<string>? Medications { get; set; } // text[] 
    public int PreviousPregnancies { get; set; }
    public string? Surgeries { get; set; }
    public List<string>? Vaccinations { get; set; } // text[] 

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
