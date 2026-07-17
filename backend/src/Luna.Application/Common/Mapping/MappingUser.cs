using Luna.Application.Common.Models;
using Luna.Domain.Entities;
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
            LastMenstrualPeriod = user.LastMenstrualPeriod,
            EstimatedDueDate = user.EstimatedDueDate,
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
}
