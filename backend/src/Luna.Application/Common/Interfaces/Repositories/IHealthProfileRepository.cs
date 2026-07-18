using Luna.Domain.Entities;

namespace Luna.Application.Common.Interfaces.Repositories;

public interface IHealthProfileRepository
{
    Task<HealthProfile?> GetByUserIdAsync(Guid userId);
    Task<HealthProfile> CreateAsync(HealthProfile profile);
    Task<HealthProfile> UpdateAsync(HealthProfile profile);
}
