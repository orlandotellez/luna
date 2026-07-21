namespace Luna.Application.Common.Models.Pregnancy;

public record PregnancyWeeksResponseDto
{
    public List<PregnancyWeekProgressDto> Weeks { get; init; } = new();
}
