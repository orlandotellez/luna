using Luna.Domain.Entities;

namespace Luna.Application.Common.Interfaces.Repositories;

public interface ISymptomEntryRepository
{
    Task<SymptomEntry> CreateAsync(SymptomEntry symptom);
}
