using Luna.Domain.Entities.Users;

namespace Luna.Application.Common.Interfaces.Repositories;

public interface IUserProfileRepository
{
    Task<UserProfile?> GetByUserIdAsync(Guid userId);
    Task<UserProfile?> GetByUserNameAsync(string userName);
    Task<UserProfile> CreateAsync(UserProfile profile);
    Task<UserProfile> UpdateAsync(UserProfile profile);
}
