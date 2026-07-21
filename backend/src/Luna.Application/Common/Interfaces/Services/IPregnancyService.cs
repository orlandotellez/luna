using Luna.Application.Common.Models.Pregnancy;

namespace Luna.Application.Common.Interfaces.Services;

public interface IPregnancyService
{
    Task<PregnancyRegistrationResponseDto> RegisterPregnancyAsync(Guid userId, RegisterPregnancyRequest request);
}
