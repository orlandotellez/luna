using Luna.Domain.Entities.Cycle;

namespace Luna.Application.Common.Interfaces.Repositories;

public interface ISymptomEntryRepository
{
    Task<SymptomEntry> CreateAsync(SymptomEntry symptom);
}
