using Luna.Application.Common.Interfaces.Repositories.Users;
using Luna.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Luna.Infrastructure.Persistence.Repositories.Users;

public class UserProfileRepository : IUserProfileRepository
{
    private readonly ApplicationDbContext _context;

    public UserProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetByUserIdAsync(Guid userId)
    {
        return await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == userId);
    }

    public async Task<UserProfile?> GetByUserNameAsync(string userName)
    {
        return await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserName == userName);
    }

    public async Task<UserProfile> CreateAsync(UserProfile profile)
    {
        await _context.UserProfiles.AddAsync(profile);
        await _context.SaveChangesAsync();
        return profile;
    }

    public async Task<UserProfile> UpdateAsync(UserProfile profile)
    {
        _context.UserProfiles.Update(profile);
        await _context.SaveChangesAsync();
        return profile;
    }
}
