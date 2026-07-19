using Microsoft.EntityFrameworkCore;
using Luna.Application.Common.Interfaces.Repositories.Users;
using Luna.Domain.Entities.Users;

namespace Luna.Infrastructure.Persistence.Repositories.Users;

public class HealthProfileRepository : IHealthProfileRepository
{
    private readonly ApplicationDbContext _context;

    public HealthProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HealthProfile?> GetByUserIdAsync(Guid userId)
    {
        return await _context.HealthProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<HealthProfile> CreateAsync(HealthProfile profile)
    {
        await _context.HealthProfiles.AddAsync(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<HealthProfile> UpdateAsync(HealthProfile profile)
    {
        _context.HealthProfiles.Update(profile);
        await _context.SaveChangesAsync();
        return profile;
    }
}
