using Luna.Application.Common.Models.Pregnancy;

namespace Luna.Application.Common.Interfaces.Services;

public interface IPregnancyContentService
{
    Task<PregnancyWeekContentDto?> GetWeekAsync(int weekNumber);
    Task<List<PregnancyWeekContentDto>> GetAllWeeksAsync();
    Task<PregnancyWeeksResponseDto> GetWeeksProgressAsync(int? currentWeek);
}
