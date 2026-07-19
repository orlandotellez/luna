using Luna.Application.Common.Models.Cycle;
using Luna.Domain.Entities;

namespace Luna.Application.Common.Mapping;

public static class MappingCycle
{
    public static PeriodEntryDto MapPeriodEntryToDto(this PeriodEntry period)
    {
        return new PeriodEntryDto
        {
            Id = period.Id,
            StartDate = period.StartDate,
            EndDate = period.EndDate,
            Notes = period.Notes,
            CreatedAt = period.CreatedAt
        };
    }
}
