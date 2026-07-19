using Microsoft.EntityFrameworkCore;
using Luna.Application.Common.Interfaces.Repositories.Cycle;
using Luna.Domain.Entities.Cycle;

namespace Luna.Infrastructure.Persistence.Repositories;

public class PeriodEntryRepository : IPeriodEntryRepository
{
    private readonly ApplicationDbContext _context;

    public PeriodEntryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PeriodEntry?> GetByIdAsync(Guid id)
    {
        return await _context.PeriodEntries
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<PeriodEntry>> GetByUserIdAsync(Guid userId)
    {
        return await _context.PeriodEntries
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();
    }

    public async Task<PeriodEntry?> GetLastByUserIdAsync(Guid userId)
    {
        return await _context.PeriodEntries
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.StartDate)
            .FirstOrDefaultAsync();
    }

    public async Task<PeriodEntry> CreateAsync(PeriodEntry period)
    {
        await _context.PeriodEntries.AddAsync(period);
        await _context.SaveChangesAsync();
        return period;
    }

    public async Task<PeriodEntry> UpdateAsync(PeriodEntry period)
    {
        _context.PeriodEntries.Update(period);
        await _context.SaveChangesAsync();
        return period;
    }
}
