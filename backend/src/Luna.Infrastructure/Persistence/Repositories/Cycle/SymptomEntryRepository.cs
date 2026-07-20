using Microsoft.EntityFrameworkCore;
using Luna.Application.Common.Interfaces.Repositories.Cycle;
using Luna.Domain.Entities.Cycle;

namespace Luna.Infrastructure.Persistence.Repositories.Cycle;

public class SymptomEntryRepository : ISymptomEntryRepository
{
    private readonly ApplicationDbContext _context;

    public SymptomEntryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SymptomEntry> CreateAsync(SymptomEntry symptom)
    {
        await _context.SymptomEntries.AddAsync(symptom);
        await _context.SaveChangesAsync();
        return symptom;
    }

    public async Task<List<SymptomEntry>> GetByUserIdInRangeAsync(Guid userId, DateOnly rangeStart, DateOnly rangeEnd)
    {
        return await _context.SymptomEntries
            .Where(s => s.UserId == userId && s.Date >= rangeStart && s.Date <= rangeEnd)
            .OrderBy(s => s.Date)
            .ToListAsync();
    }
}
