using Luna.Domain.Entities.Cycle;

namespace Luna.Application.Common.Interfaces.Repositories;

public interface IPeriodEntryRepository
{
    Task<PeriodEntry?> GetByIdAsync(Guid id);
    Task<List<PeriodEntry>> GetByUserIdAsync(Guid userId);
    Task<PeriodEntry?> GetLastByUserIdAsync(Guid userId);
    Task<PeriodEntry> CreateAsync(PeriodEntry period);
    Task<PeriodEntry> UpdateAsync(PeriodEntry period);
}
