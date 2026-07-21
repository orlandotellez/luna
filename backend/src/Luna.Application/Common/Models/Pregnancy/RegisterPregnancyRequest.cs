
namespace Luna.Application.Common.Models.Pregnancy;

public record RegisterPregnancyRequest
{
    public DateOnly LastMenstrualPeriod { get; set; }
    public bool? IsFirstPregnancy { get; set; }
}
