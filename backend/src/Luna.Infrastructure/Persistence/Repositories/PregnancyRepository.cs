using Microsoft.EntityFrameworkCore;
using Luna.Application.Common.Interfaces.Repositories;
using Luna.Domain.Entities.Cycle;

namespace Luna.Infrastructure.Persistence.Repositories;

public class PregnancyRepository : IPregnancyRepository
{
    private readonly ApplicationDbContext _context;

    public PregnancyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Pregnancy?> GetActiveByUserIdAsync(Guid userId)
    {
        return await _context.Pregnancies
            .FirstOrDefaultAsync(p => p.UserId == userId && p.IsActive);
    }

    public async Task<List<Pregnancy>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Pregnancies
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Pregnancy> CreateAsync(Pregnancy pregnancy)
    {
        await _context.Pregnancies.AddAsync(pregnancy);
        await _context.SaveChangesAsync();
        return pregnancy;
    }

    public async Task<Pregnancy> UpdateAsync(Pregnancy pregnancy)
    {
        _context.Pregnancies.Update(pregnancy);
        await _context.SaveChangesAsync();
        return pregnancy;
    }
}
