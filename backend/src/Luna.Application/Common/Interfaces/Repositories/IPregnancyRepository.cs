using Luna.Domain.Entities;

namespace Luna.Application.Common.Interfaces.Repositories;

public interface IPregnancyRepository
{
    Task<Pregnancy?> GetActiveByUserIdAsync(Guid userId);
    Task<List<Pregnancy>> GetByUserIdAsync(Guid userId);
    Task<Pregnancy> CreateAsync(Pregnancy pregnancy);
    Task<Pregnancy> UpdateAsync(Pregnancy pregnancy);
}
