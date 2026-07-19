using Luna.Application.Common.Interfaces.Services;
using Luna.Application.Common.Interfaces.Repositories;
using Luna.Application.Common.Models.User;
using Luna.Application.Common.Models.Cycle;
using Luna.Application.Common.Mapping;
using Luna.Application.Common.Helpers;
using Luna.Domain.Exceptions;
using Luna.Domain.Enums;
using Luna.Domain.Entities.Cycle;
using Luna.Domain.Entities.Users;

namespace Luna.Application.Features.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IPregnancyRepository _pregnancyRepository;
    private readonly IHealthProfileRepository _healthProfileRepository;
    private readonly IPeriodEntryRepository _periodEntryRepository;
    private readonly ISymptomEntryRepository _symptomEntryRepository;

    public UserService(
        IUserRepository userRepository,
        IUserProfileRepository userProfileRepository,
        IPregnancyRepository pregnancyRepository,
        IHealthProfileRepository healthProfileRepository,
        IPeriodEntryRepository periodEntryRepository,
ISymptomEntryRepository symptomEntryRepository
        )
    {
        _userRepository = userRepository;
        _userProfileRepository = userProfileRepository;
        _pregnancyRepository = pregnancyRepository;
        _healthProfileRepository = healthProfileRepository;
        _periodEntryRepository = periodEntryRepository;
        _symptomEntryRepository = symptomEntryRepository;
    }

    public async Task<UserDto> GetMyProfileAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        user.Profile = await _userProfileRepository.GetByUserIdAsync(userId);
        user.HealthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);

        // load pregnancies
        var pregnancies = await _pregnancyRepository.GetByUserIdAsync(userId);

        return user.MapUserToDto();
    }

    public async Task<UserDto> UpdateAvatarAsync(Guid userId, string imageUrl)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        user.Image = imageUrl;
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);

        user.Profile = await _userProfileRepository.GetByUserIdAsync(userId);
        user.HealthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);

        return user.MapUserToDto();
    }

    public async Task<UserDto> UpdateUserProfileAsync(Guid userId, UpdateUserProfileRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        var profile = await _userProfileRepository.GetByUserIdAsync(userId);
        if (profile is null)
        {
            profile = new UserProfile
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _userProfileRepository.CreateAsync(profile);
        }


        if (request.UserName is not null && !string.Equals(request.UserName, profile.UserName, StringComparison.OrdinalIgnoreCase))
        {
            var existingProfile = await _userProfileRepository.GetByUserNameAsync(request.UserName);

            if (existingProfile is not null) throw AppExceptions.Conflict("UserName is already taken");

            profile.UserName = request.UserName;
        }

        if (request.Name is not null) user.Name = request.Name;
        if (request.Phone is not null) user.Phone = request.Phone;
        if (request.Bio is not null) profile.Bio = request.Bio;

        user.UpdatedAt = DateTime.UtcNow;
        profile.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        await _userProfileRepository.UpdateAsync(profile);

        return user.MapUserToDto();
    }

    public async Task<UserDto> UpdateLifeStageAsync(Guid userId, UpdateLifeStageRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        user.LifeStage = request.LifeStage;

        if (request.LifeStage == LifeStage.Pregnancy)
        {
            // Buscar embarazo activo o crear uno nuevo
            var pregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);

            if (pregnancy is null)
            {
                pregnancy = new Pregnancy
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    LastMenstrualPeriod = request.LastMenstrualPeriod,
                    EstimatedDueDate = request.EstimatedDueDate!.Value,
                    CurrentWeek = PregnancyHelper.CalculateCurrentWeek(request.EstimatedDueDate!.Value),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _pregnancyRepository.CreateAsync(pregnancy);
            }
            else
            {
                pregnancy.LastMenstrualPeriod = request.LastMenstrualPeriod ?? pregnancy.LastMenstrualPeriod;
                pregnancy.EstimatedDueDate = request.EstimatedDueDate ?? pregnancy.EstimatedDueDate;
                pregnancy.UpdatedAt = DateTime.UtcNow;
                await _pregnancyRepository.UpdateAsync(pregnancy);
            }
        }
        else
        {
            // Si cambia a otra etapa, desactivar embarazo activo
            var activePregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);
            if (activePregnancy is not null)
            {
                activePregnancy.IsActive = false;
                activePregnancy.EndedAt = DateTime.UtcNow;
                activePregnancy.UpdatedAt = DateTime.UtcNow;
                await _pregnancyRepository.UpdateAsync(activePregnancy);
            }
        }

        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Cargar relaciones para el DTO
        user.Profile = await _userProfileRepository.GetByUserIdAsync(userId);
        user.HealthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);

        var result = user.MapUserToDto();
        return result;
    }

    public async Task<HealthProfileDto?> GetHealthProfileAsync(Guid userId)
    {
        var healthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);

        return healthProfile.MapHealthProfileToDto();
    }

    public async Task<HealthProfileDto> UpdateHealthProfileAsync(Guid userId, UpdateHealthProfileRequest request)
    {
        var healthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);
        bool isNew = false;

        if (healthProfile is null)
        {
            healthProfile = new HealthProfile
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            isNew = true;
        }

        if (request.HasRegularCycle is not null) healthProfile.HasRegularCycle = request.HasRegularCycle;
        if (request.CycleLengthDays is not null) healthProfile.CycleLengthDays = request.CycleLengthDays;
        if (request.PeriodLengthDays is not null) healthProfile.PeriodLengthDays = request.PeriodLengthDays;

        healthProfile.HasEndometriosis = request.HasEndometriosis ?? false;
        healthProfile.HasPcos = request.HasPcos ?? false;
        healthProfile.HasThyroidIssues = request.HasThyroidIssues ?? false;
        healthProfile.HasGestationalDiabetes = request.HasGestationalDiabetes ?? false;
        healthProfile.HasFibroids = request.HasFibroids ?? false;
        healthProfile.HasHypertension = request.HasHypertension ?? false;

        if (request.Allergies is not null) healthProfile.Allergies = request.Allergies;
        if (request.Medications is not null) healthProfile.Medications = request.Medications;
        if (request.PreviousPregnancies is not null) healthProfile.PreviousPregnancies = request.PreviousPregnancies.Value;
        if (request.Surgeries is not null) healthProfile.Surgeries = request.Surgeries;
        if (request.Vaccinations is not null) healthProfile.Vaccinations = request.Vaccinations;

        healthProfile.UpdatedAt = DateTime.UtcNow;

        if (isNew)
            await _healthProfileRepository.CreateAsync(healthProfile);
        else
            await _healthProfileRepository.UpdateAsync(healthProfile);

        return healthProfile.MapHealthProfileToDto()!;
    }

    public async Task<CycleCurrentDto> GetCurrentCycleAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        var healthProfile = await _healthProfileRepository.GetByUserIdAsync(userId);

        var activePregnancy = await _pregnancyRepository.GetActiveByUserIdAsync(userId);

        var result = new CycleCurrentDto
        {
            LifeStage = user.LifeStage ?? LifeStage.ActiveCycle,
            CycleLengthDays = healthProfile?.CycleLengthDays,
            PeriodLengthDays = healthProfile?.PeriodLengthDays,
            HasRegularCycle = healthProfile?.HasRegularCycle,
            ActivePregnancy = activePregnancy.MapPregnancyToDto(),
            Predictions = null
        };

        if (healthProfile?.HasRegularCycle == true && healthProfile.CycleLengthDays.HasValue)
        {
            result.Predictions = new CyclePredictionDto
            {
                CurrentPhase = "regular_cycle_configured",
            };
        }

        if (activePregnancy is not null)
        {
            result.LifeStage = LifeStage.Pregnancy;
        }

        return result;
    }

    public async Task<PeriodEntryDto> RegisterPeriodAsync(Guid userId, RegisterPeriodRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        // Validar que EndDate no sea anterior a StartDate
        if (request.EndDate.HasValue && request.EndDate.Value < request.StartDate)
            throw AppExceptions.BadRequest("EndDate cannot be before StartDate");

        var period = new PeriodEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _periodEntryRepository.CreateAsync(period);

        return period.MapPeriodEntryToDto();
    }

    public async Task<SymptomEntryDto> RegisterSymptomAsync(Guid userId, RegisterSymptomRequest request)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null) throw AppExceptions.NotFound("User NotFound");

        // Validate severity
        if (request.Severity.HasValue && (request.Severity.Value < 1 || request.Severity.Value > 10))
            throw AppExceptions.BadRequest("Severity must be between 1 and 10");

        var symptom = new SymptomEntry
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Date = request.Date,
            Symptom = request.Symptom,
            Severity = request.Severity,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _symptomEntryRepository.CreateAsync(symptom);

        return symptom.MapSymptomEntryToDto();
    }
}


