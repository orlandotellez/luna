using Luna.Application.Common.Models.User;
using Luna.Domain.Entities.Users;
using Luna.Domain.Exceptions;

namespace Luna.Application.Common.Mapping;

public static class MappingUser
{
    public static UserDto MapUserToDto(this User user)
    {
        if (user == null) throw AppExceptions.UnprocessableEntity(nameof(user));

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            EmailVerified = user.EmailVerified,
            Phone = user.Phone,
            Image = user.Image,
            Role = user.Role,
            LifeStage = user.LifeStage,
            ActivePregnancy = user.Pregnancies?.FirstOrDefault(p => p.IsActive).MapPregnancyToDto(),
            HealthProfile = user.HealthProfile.MapHealthProfileToDto(),
            IsActive = user.IsActive,
            LastSeenAt = user.LastSeenAt,
            CreatedAt = user.CreatedAt,
            DeletedAt = user.DeletedAt,
            DeletedByUserId = user.DeletedByUserId,
            DeletedByName = user.DeletedByName,
            Profile = user.Profile?.MapUserProfileToDto(),
        };
    }

    public static UserProfileDto MapUserProfileToDto(this UserProfile profile)
    {
        if (profile == null) throw AppExceptions.UnprocessableEntity(nameof(profile));

        return new UserProfileDto
        {
            UserName = profile.UserName,
            Bio = profile.Bio,
            DateOfBirth = profile.DateOfBirth,
            DepartmentId = profile.DepartmentId,
            MunicipalityId = profile.MunicipalityId
        };
    }


    public static HealthProfileDto? MapHealthProfileToDto(this HealthProfile? profile)
    {
        if (profile is null) return null;

        return new HealthProfileDto
        {
            HasRegularCycle = profile.HasRegularCycle,
            CycleLengthDays = profile.CycleLengthDays,
            PeriodLengthDays = profile.PeriodLengthDays,
            HasEndometriosis = profile.HasEndometriosis ?? false,
            HasPcos = profile.HasPcos ?? false,
            HasThyroidIssues = profile.HasThyroidIssues ?? false,
            HasGestationalDiabetes = profile.HasGestationalDiabetes ?? false,
            HasFibroids = profile.HasFibroids ?? false,
            HasHypertension = profile.HasHypertension ?? false,
            Allergies = profile.Allergies,
            Medications = profile.Medications,
            PreviousPregnancies = profile.PreviousPregnancies,
            Surgeries = profile.Surgeries,
            Vaccinations = profile.Vaccinations
        };
    }

}
