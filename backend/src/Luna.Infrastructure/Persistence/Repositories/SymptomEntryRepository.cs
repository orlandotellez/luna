using Microsoft.EntityFrameworkCore;
using Luna.Application.Common.Interfaces.Repositories;
using Luna.Domain.Entities;

namespace Luna.Infrastructure.Persistence.Repositories;

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
}
