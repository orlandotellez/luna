namespace Luna.Application.Common.Models.Pregnancy;

public record PregnancyRegistrationResponseDto
{
    public Guid PregnancyId { get; set; }
    public int CurrentWeek { get; set; }
    public DateOnly EstimatedDueDate { get; set; }
    public int Trimester { get; set; }
    public int WeeksRemaining { get; set; }
}
