using Luna.Domain.Entities.Cycle;

namespace Luna.Application.Common.Interfaces.Repositories.Cycle;

public interface ISymptomEntryRepository
{
    Task<SymptomEntry> CreateAsync(SymptomEntry symptom);
}
