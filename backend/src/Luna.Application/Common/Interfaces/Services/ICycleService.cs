using Luna.Application.Common.Models.User;
using Luna.Application.Common.Models.Cycle;

namespace Luna.Application.Common.Interfaces.Services;

public interface ICycleService
{
    Task<CycleCurrentDto> GetCurrentCycleAsync(Guid userId);
    Task<PeriodEntryDto> RegisterPeriodAsync(Guid userId, RegisterPeriodRequest request);

}
