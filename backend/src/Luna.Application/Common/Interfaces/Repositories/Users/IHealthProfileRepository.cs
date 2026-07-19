using Luna.Domain.Entities.Users;

namespace Luna.Application.Common.Interfaces.Repositories.Users;

public interface IHealthProfileRepository
{
    Task<HealthProfile?> GetByUserIdAsync(Guid userId);
    Task<HealthProfile> CreateAsync(HealthProfile profile);
    Task<HealthProfile> UpdateAsync(HealthProfile profile);
}
