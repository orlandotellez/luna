namespace Luna.Application.Common.Models.Pregnancy;

public record PregnancyWeekContentDto
{
    public int WeekNumber { get; init; }

    public int Trimester => WeekNumber <= 13 ? 1 : WeekNumber <= 27 ? 2 : 3;

    public string Title { get; init; } = string.Empty;
    public decimal? BabySizeCm { get; init; }
    public int? BabyWeightG { get; init; }
    public List<string> Highlights { get; init; } = new();
    public List<string> TipsForMom { get; init; } = new();
    public List<string> Alerts { get; init; } = new();
}
