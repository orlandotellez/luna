using Luna.Domain.Enums;

namespace Luna.Application.Common.Models.Cycle;

public record CycleHistoryItemDto
{
    public Guid Id { get; set; }
    public int CycleNumber { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int? PeriodLengthDays { get; set; }
    public int? CycleLengthDays { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public record CycleHistoryDto
{
    public Guid UserId { get; set; }
    public LifeStage LifeStage { get; set; }
    public int TotalCycles { get; set; }
    public double? AverageCycleLengthDays { get; set; }
    public double? AveragePeriodLengthDays { get; set; }
    public List<CycleHistoryItemDto> Cycles { get; set; } = new();
}
