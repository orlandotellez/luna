using Luna.Application.Common.Models.Cycle;
using Luna.Domain.Entities.Cycle;

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

    public static SymptomEntryDto MapSymptomEntryToDto(this SymptomEntry symptom)
    {
        return new SymptomEntryDto
        {
            Id = symptom.Id,
            Date = symptom.Date,
            Symptom = symptom.Symptom,
            Severity = symptom.Severity,
            Notes = symptom.Notes,
            CreatedAt = symptom.CreatedAt
        };
    }
}
