using Luna.Application.Common.Models.User;
using Luna.Domain.Entities.Users;

namespace Luna.Application.Common.Interfaces.Repositories.Users;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(UserFilter filter);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User> CreateAsync(User user);
    Task<User> UpdateAsync(User user);
    Task SoftDeleteAsync(Guid id, Guid deletedByUserId, string deletedByName);
    Task RestoreAsync(Guid id);
}

