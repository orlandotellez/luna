using Luna.Application.Common.Models.Cycle;
using Luna.Domain.Entities.Cycle;
using Luna.Domain.Entities.Pregnancies;
using Luna.Application.Common.Models.User;

namespace Luna.Application.Common.Mapping;

public static class MappingCycle
{
    public static PregnancyDto? MapPregnancyToDto(this Pregnancy? pregnancy)
    {
        if (pregnancy is null) return null;

        return new PregnancyDto
        {
            Id = pregnancy.Id,
            LastMenstrualPeriod = pregnancy.LastMenstrualPeriod,
            EstimatedDueDate = pregnancy.EstimatedDueDate,
            CurrentWeek = pregnancy.CurrentWeek,
            IsFirstPregnancy = pregnancy.IsFirstPregnancy,
            PregnancyCount = pregnancy.PregnancyCount,
            IsActive = pregnancy.IsActive,
            EndedAt = pregnancy.EndedAt,
            Notes = pregnancy.Notes
        };
    }

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
